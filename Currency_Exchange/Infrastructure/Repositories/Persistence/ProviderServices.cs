using Application.Contracts.Persistence;
using Application.Dtos.AccountDtos;
using Application.Dtos.OthersAccountDto;
using Application.Dtos.TransactionDtos;
using Application.Statics;
using AutoMapper;
using Domain.Entities;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using System.Transactions;
using Infrastructure.Migrations;
using Transaction = Domain.Entities.Transaction;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Infrastructure.Repositories.Persistence
{
    public class ProviderServices : IProviderServices
    {
        private readonly ICurrencyServices _currencyServices;
        private readonly IAccountServices _accountServices;
        private readonly IOthersAccountServices _othersAccountServices;
        private readonly CurrencyDbContext _context;
        private readonly IMapper _mapper;

        public ProviderServices(ICurrencyServices currencyServices, IAccountServices accountServices, IOthersAccountServices othersAccountServices, CurrencyDbContext context, IMapper mapper)
        {
            _currencyServices = currencyServices;
            _accountServices = accountServices;
            _othersAccountServices = othersAccountServices;
            _context = context;
            _mapper = mapper;
        }

        public async Task<TransactionDto> GetTransaction(int idTransaction)
        {
            return _mapper.Map<TransactionDto>(await _context.Transactions.SingleOrDefaultAsync(x => x.TransactionId.Equals(idTransaction)));
        }

        public async Task<ConfirmTransactionDto> GetConfirmTransaction(int idTransaction, string userId)
        {
            var confirmTransaction = _mapper.Map<ConfirmTransactionDto>(await _context.Transactions.SingleOrDefaultAsync(x => x.UserId != null && x.TransactionId.Equals(idTransaction) && x.UserId.Equals(userId)));
            // Get Name Of Accounts
            confirmTransaction.FromAccountId = await GetNameAccountForTransaction(int.Parse(confirmTransaction.FromAccountId));
            confirmTransaction.ToAccountId = await GetNameAccountForTransaction(int.Parse(confirmTransaction.ToAccountId));
            return confirmTransaction;
        }

        public async Task<TransactionDetailDto> GetDetailTransaction(int idTransaction, string userId)
        {
            var confirmTransaction = _mapper.Map<TransactionDetailDto>(await _context.Transactions.SingleOrDefaultAsync(x => x.UserId != null && x.TransactionId.Equals(idTransaction) && x.UserId.Equals(userId)));
            // Get Name Of Accounts
            confirmTransaction.FromAccountId = await GetNameAccountForTransaction(int.Parse(confirmTransaction.FromAccountId));
            confirmTransaction.ToAccountId = await GetNameAccountForTransaction(int.Parse(confirmTransaction.ToAccountId));
            return confirmTransaction;
        }

        public async Task<List<TransactionDto>> GetListTransactions(int fromIdAccount)
        {
            return _mapper.Map<List<TransactionDto>>(await _context.Transactions.Where(x => x.FromAccountId.Equals(fromIdAccount)).ToListAsync());
        }

        public async Task<List<UsersTransactionsDto>> GetListTransactionsForAdmin()
        {
            return _mapper.Map<List<UsersTransactionsDto>>(await _context.Transactions.ToListAsync());
        }

        public async Task<bool> CancelTransaction(int transactionId)
        {
            var transaction = await _context.Transactions.SingleOrDefaultAsync(x => x.TransactionId.Equals(transactionId));
            if (transaction == null)
            {
                return false;
            }
            transaction.Status = StatusEnum.Cancelled;
            _context.Transactions.Update(transaction);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> TransformCurrency(CreateTransactionDtos transactionVM, string username)
        {
            //validate
            var isUserAccount = await _accountServices.IsAccountForUser(username, transactionVM.SelfAccountId);
            if (!isUserAccount) return 0;
            var otherAccount = await _accountServices.GetAccountByIdAsync(int.Parse(transactionVM.OthersAccountIdAsString));
            if (otherAccount == null) return 0;
            // processes
            var transaction = _mapper.Map<Transaction>(transactionVM);
            transaction.Status = StatusEnum.Pending;
            transaction.ExchangeRate = await _currencyServices.GetPriceRateExchange(transactionVM.FromCurrency, otherAccount.Currency);
            var isUsersSelfAccount = await _accountServices.IsAccountForUser(username, int.Parse(transactionVM.OthersAccountIdAsString));
            if (!isUsersSelfAccount)
            {
                transaction.Fee = await _currencyServices.GetTransformFeeCurrencyAsync(transactionVM.FromCurrency, otherAccount.Currency, transactionVM.Amount);
                transaction.Fee += await _currencyServices.GetExchangeFeeCurrencyAsync(transactionVM.FromCurrency, otherAccount.Currency, transactionVM.Amount);
            }
            transaction.ToCurrency = otherAccount.Currency;
            transaction.ToAccountId = int.Parse(transactionVM.OthersAccountIdAsString);
            await _context.Transactions.AddAsync(transaction);
            var queryResult = await _context.SaveChangesAsync();
            return queryResult > 0 ? transaction.TransactionId : 0;
        }


        public async Task<bool> ConfirmTransaction(int transactionId, string username, bool isConfirm)
        {
            using var safeScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            // Validations
            var transaction = await _context.Transactions.SingleOrDefaultAsync(x => x.UserId != null && x.TransactionId.Equals(transactionId) && x.UserId.Equals(username));
            if (transaction == null) return false;
            if (isConfirm == false)
            {
                await CancelTransaction(transactionId);
                safeScope.Complete();
                return false;
            }
            if (transaction.Status.Equals(StatusEnum.Completed) || transaction.Status.Equals(StatusEnum.Cancelled)) return false;
            var expiredDateTime = transaction.CreatedAt.AddMinutes(10);
            if (expiredDateTime < DateTime.UtcNow)
            {
                await CancelTransaction(transactionId);
                safeScope.Complete();
                return false;
            }
            var checkMaxAmountDaily = await CheckMaxOfTransaction(username, transaction.FromAccountId, transaction.Amount, transaction.FromCurrency);
            if (!checkMaxAmountDaily)
            {
                await CancelTransaction(transactionId);
                safeScope.Complete();
                return false;
            }
            var userAccount = await _accountServices.GetAccountByIdAsync(transaction.UserId, transaction.FromAccountId);
            var toAccount = await _accountServices.GetAccountByIdAsync(transaction.ToAccountId);
            if (toAccount == null || userAccount == null)
            {
                await CancelTransaction(transactionId);
                safeScope.Complete();
                return false;
            }

            // processes
            if (!toAccount.Currency.Equals(transaction.FromCurrency))
            {
                toAccount.Balance += await _currencyServices.CurrencyConvertor(userAccount.Currency, toAccount.Currency, transaction.Amount);
            }
            else
            {
                toAccount.Balance += transaction.Amount;
            }
            var totalPriceToPay = transaction.Fee + transaction.Amount;
            userAccount.Balance -= totalPriceToPay;
            var toAccountUpdate = await _accountServices.UpdateMoneyAccount(_mapper.Map<UpdateAccountViewModel>(toAccount));
            var userAccountUpdate = await _accountServices.UpdateAccount(_mapper.Map<UpdateAccountViewModel>(userAccount), transaction.UserId);
            if (userAccountUpdate == 0 || toAccountUpdate == 0) return false;
            transaction.Outer = true;
            transaction.Status = StatusEnum.Completed;
            transaction.CompletedAt = DateTime.UtcNow;
            transaction.DeductedAmount = totalPriceToPay;
            transaction.UserBalance = userAccount.Balance;
            _context.Transactions.Update(transaction);
            var result = await _context.SaveChangesAsync() > 0;
            if (result == false) return false;
            safeScope.Complete();
            return true;

        }

        public async Task<List<Transaction>> CanceledPendingTransactionsByTimePass(int min)
        {
            var timer = DateTime.UtcNow.AddMinutes(-min);
            var recentTransactions = await _context.Transactions
                .Where(t => t.CreatedAt >= timer && t.Status.Equals(StatusEnum.Pending))
                .ToListAsync();
            foreach (var item in recentTransactions)
            {
                item.Status = StatusEnum.Cancelled;
            }
            _context.Transactions.UpdateRange(recentTransactions);
            var queryResult = await _context.SaveChangesAsync();
            return queryResult > 0 ? recentTransactions : new List<Transaction>();
        }

        public async Task<bool> CheckMaxOfTransaction(string userId, int accountId, decimal price, string currency)
        {
            var transactions = await _context.Transactions.Where(x =>
                x.CompletedAt != null && x.UserId != null && x.Status == StatusEnum.Completed
                && x.CompletedAt.Value.Date.Equals(DateTime.UtcNow.Date)
                && x.UserId.Equals(userId)
                && x.FromAccountId.Equals(accountId)
                && x.Outer).ToListAsync();
            decimal dollarBalance = 0;
            foreach (var transaction in transactions)
            {
                // increase & withdraw
                if (transaction.ToAccountId.Equals(accountId)) continue;
                // Transform
                if (transaction.FromCurrency.Equals("USD"))
                {
                    dollarBalance += transaction.Amount;
                }
                else
                {
                    dollarBalance += await _currencyServices.CurrencyConvertor(transaction.FromCurrency, "USD", transaction.Amount);
                }
            }
            var dollarPrice = await _currencyServices.CurrencyConvertor(currency, "USD", price);
            return dollarBalance + dollarPrice <= MaximumTransaction.MaxTransaction;
        }

        public async Task<string> GetNameAccountForTransaction(int accountId)
        {
            var account = await _context.Accounts.SingleOrDefaultAsync(x => x.AccountId.Equals(accountId));
            return account != null ? $"{account.AccountName} - {account.CartNumber} " : "";
        }

        public async Task<string> GetOtherNameAccountForTransaction(int accountId)
        {
            var account = await _context.OthersAccounts.SingleOrDefaultAsync(x => x.AccountId.Equals(accountId));
            return account != null ? $"{account.AccountName} - {account.CartNumber} " : "";
        }
    }
}
