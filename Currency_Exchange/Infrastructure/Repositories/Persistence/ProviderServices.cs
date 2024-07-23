using Application.Contracts.Persistence;
using Application.Dtos.AccountDtos;
using Application.Dtos.OthersAccountDto;
using Application.Dtos.TransactionDtos;
using Application.Statics;
using AutoMapper;
using Domain.Entities;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

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
            if (!string.IsNullOrEmpty(confirmTransaction.ToAccountId))
            {
                confirmTransaction.ToAccountId = await GetNameAccountForTransaction(int.Parse(confirmTransaction.ToAccountId));
            }
            if (!string.IsNullOrEmpty(confirmTransaction.ToOtherAccountId))
            {
                confirmTransaction.ToOtherAccountId = await GetOtherNameAccountForTransaction(int.Parse(confirmTransaction.ToOtherAccountId));
            }
            return confirmTransaction;
        }

        public async Task<List<TransactionDto>> GetListTransactions(int fromIdAccount)
        {
            return _mapper.Map<List<TransactionDto>>(await _context.Transactions.Where(x => x.FromAccountId.Equals(fromIdAccount)).ToListAsync());
        }

        public async Task<List<TransactionDto>> GetListTransactionsForAdmin()
        {
            return _mapper.Map<List<TransactionDto>>(await _context.Transactions.ToListAsync());
        }

        public async Task<int> TransformCurrency(CreateTransactionDtos transactionVM, string username)
        {
            var isSelfAccount = await _accountServices.IsAccountForUser(username, transactionVM.SelfAccountId);
            if (!isSelfAccount) return 0;
            var isOtherAccount = await _othersAccountServices.IsAccountForOthers(username, transactionVM.OthersAccountId);
            if (!isOtherAccount) return 0;
            var otherAccount = await _othersAccountServices.GetOtherAccountByIdAsync(transactionVM.OthersAccountId, username);
            var transaction = _mapper.Map<Transaction>(transactionVM);
            transaction.Status = StatusEnum.Pending;
            transaction.ExchangeRate = await _currencyServices.GetPriceRateExchange(transactionVM.FromCurrency, otherAccount.Currency);
            transaction.Fee = await _currencyServices.GetTransformFeeCurrencyAsync(transactionVM.FromCurrency, otherAccount.Currency, transactionVM.Amount);
            transaction.Fee += await _currencyServices.GetExchangeFeeCurrencyAsync(transactionVM.FromCurrency, otherAccount.Currency, transactionVM.Amount);
            transaction.ToCurrency = otherAccount.Currency;
            transaction.ToOtherAccountId = transactionVM.OthersAccountId;

            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return transaction.TransactionId;
        }

       

        public async Task<int> TransformToSelfAccountCurrency(CreateTransactionDtos transactionVM, string username)
        {
            var isSelfAccount = await _accountServices.IsAccountForUser(username, transactionVM.SelfAccountId);
            if (!isSelfAccount) return 0;
            var otherSelfAccount = await _accountServices.GetAccountByIdAsync(username, transactionVM.OthersAccountId);
            var transaction = _mapper.Map<Transaction>(transactionVM);
            transaction.Status = StatusEnum.Pending;
            transaction.ToAccountId = transactionVM.OthersAccountId;
            transaction.ToCurrency = otherSelfAccount.Currency;
            transaction.ExchangeRate = await _currencyServices.GetPriceRateExchange(transactionVM.FromCurrency, otherSelfAccount.Currency);
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return transaction.TransactionId;


        }

        public async Task<bool> ConfirmTransaction(int transactionId, string username, bool isConfirm)
        {
            var transaction = await _context.Transactions.SingleOrDefaultAsync(x =>
                x.UserId != null && x.TransactionId.Equals(transactionId) && x.UserId.Equals(username));
            if (transaction == null) return false;
            if (transaction.Status.Equals(StatusEnum.Completed) || transaction.Status.Equals(StatusEnum.Cancelled)) return false;
            var expiredDateTime = transaction.CreatedAt.AddMinutes(10);
            if (expiredDateTime < DateTime.UtcNow)
            {
                return false;
            }
            var checkMaxAmount= await CheckMaxOfTransaction(username,transaction.Amount);
            if (!checkMaxAmount)
            {
                return false;
            }
            transaction.Status = isConfirm ? StatusEnum.Completed : StatusEnum.Cancelled;
            transaction.CompletedAt = DateTime.UtcNow;

            var account = await _accountServices.GetAccountByIdAsync(transaction.UserId, transaction.FromAccountId);
            if (transaction.ToAccountId!=null)
            {
                // Our Self Account
                var otherSelfAccount = await _accountServices.GetAccountByIdAsync(username, transaction.ToAccountId.Value);
                account.Balance -= transaction.ExchangeRate + transaction.Amount;
                if (!otherSelfAccount.Currency.Equals(transaction.FromCurrency))
                {
                    otherSelfAccount.Balance += await _currencyServices.CurrencyConvertor(transaction.FromCurrency, otherSelfAccount.Currency, transaction.Amount);
                }
                else
                {
                    otherSelfAccount.Balance += transaction.Amount;
                }
                var selfOtherAccountUpdate = await _accountServices.UpdateAccount(_mapper.Map<UpdateAccountViewModel>(otherSelfAccount), transaction.UserId);
                if (selfOtherAccountUpdate == 0) return false;
            }
            else
            {
                // Other Account
                var otherAccount = await _othersAccountServices.GetOtherAccountByIdAsync(transaction.ToOtherAccountId.Value, username);
                var totalPriceToPay = transaction.ExchangeRate + transaction.Fee + transaction.Amount;
                account.Balance -= totalPriceToPay;
                if (!otherAccount.Currency.Equals(transaction.FromCurrency))
                {

                    otherAccount.Balance += await _currencyServices.CurrencyConvertor(transaction.FromCurrency, otherAccount.Currency, transaction.Amount);
                }
                else
                {
                    otherAccount.Balance += transaction.Amount;
                }
                var otherAccountUpdate = await _othersAccountServices.UpdateOthersAccountAsync(_mapper.Map<UpdateOtherAccountViewModel>(otherAccount), transaction.UserId);
                if (otherAccountUpdate == 0) return false;

            }
            var accountUpdate = await _accountServices.UpdateAccount(_mapper.Map<UpdateAccountViewModel>(account), transaction.UserId);
            if (accountUpdate == 0) return false;
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
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
            await _context.SaveChangesAsync();
            return recentTransactions;
        }

        

        public async Task<bool> CheckMaxOfTransaction(string userId,decimal price)
        {
            var transactions =await _context.Transactions.Where(x =>
                x.CompletedAt != null && x.UserId != null && x.Status == StatusEnum.Completed && x.CompletedAt.Value.Date.Equals(DateTime.UtcNow.Date) &&
                x.UserId.Equals(userId)).ToListAsync();
            var balance = transactions.Sum(x => x.Amount);
            if (balance+price> MaximumTransaction.MaxTransaction)
            {
                return false;
            }
            return true;
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
