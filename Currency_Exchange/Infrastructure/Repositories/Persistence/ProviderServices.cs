using Application.Contracts.Persistence;
using Application.Dtos.AccountDtos;
using Application.Dtos.TransactionDtos;
using Application.Statics;
using AutoMapper;
using Domain.Entities;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using Application.Dtos.ErrorsDtos;
using Transaction = Domain.Entities.Transaction;

namespace Infrastructure.Repositories.Persistence
{
    public class ProviderServices : IProviderServices
    {
        private readonly ICurrencyServices _currencyServices;
        private readonly IAccountServices _accountServices;
        private readonly CurrencyDbContext _context;
        private readonly IMapper _mapper;

        public ProviderServices(ICurrencyServices currencyServices, IAccountServices accountServices, CurrencyDbContext context, IMapper mapper)
        {
            _currencyServices = currencyServices;
            _accountServices = accountServices;
            _context = context;
            _mapper = mapper;
        }

        public async Task<TransactionDto> GetTransactionAsync(int idTransaction)
        {
            return _mapper.Map<TransactionDto>(await _context.Transactions.SingleOrDefaultAsync(x => x.TransactionId.Equals(idTransaction)));
        }

        public async Task<ConfirmTransactionDto> GetConfirmTransactionAsync(int idTransaction, string userId)
        {
            var confirmTransaction = _mapper.Map<ConfirmTransactionDto>(await _context.Transactions.SingleOrDefaultAsync(x => x.UserId != null && x.TransactionId.Equals(idTransaction) && x.UserId.Equals(userId)));
            // Get Name Of Accounts
            confirmTransaction.FromAccountId = await GetNameAccountForTransaction(int.Parse(confirmTransaction.FromAccountId));
            confirmTransaction.ToAccountId = await GetNameAccountForTransaction(int.Parse(confirmTransaction.ToAccountId));
            return confirmTransaction;
        }

        public async Task<TransactionDetailDto> GetDetailTransactionAsync(int idTransaction, string userId)
        {
            var confirmTransaction = _mapper.Map<TransactionDetailDto>(await _context.Transactions.SingleOrDefaultAsync(x => x.UserId != null && x.TransactionId.Equals(idTransaction) && x.UserId.Equals(userId)));
            // Get Name Of Accounts
            confirmTransaction.FromAccountId = await GetNameAccountForTransaction(int.Parse(confirmTransaction.FromAccountId));
            confirmTransaction.ToAccountId = await GetNameAccountForTransaction(int.Parse(confirmTransaction.ToAccountId));
            return confirmTransaction;
        }

        public async Task<List<TransactionDto>> GetListTransactionsAsync(int fromIdAccount)
        {
            return _mapper.Map<List<TransactionDto>>(await _context.Transactions.Where(x => x.FromAccountId.Equals(fromIdAccount)).ToListAsync());
        }

        public async Task<List<UsersTransactionsDto>> GetListTransactionsForAdminAsync()
        {
            return _mapper.Map<List<UsersTransactionsDto>>(await _context.Transactions.ToListAsync());
        }

        public async Task<bool> CancelTransactionAsync(int transactionId)
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

        public async Task<int> TransformCurrencyAsync(CreateTransactionDtos transactionVM, string username)
        {
            //validate
            var isUserAccount = await _accountServices.IsAccountForUserAsync(username, transactionVM.SelfAccountId);
            if (!isUserAccount) return 0;
            var otherAccount = await _accountServices.GetAccountByIdAsync(int.Parse(transactionVM.OthersAccountIdAsString));
            if (otherAccount == null) return 0;
            // processes
            var transaction = _mapper.Map<Transaction>(transactionVM);
            transaction.Status = StatusEnum.Pending;

            transaction.ExchangeRate = (decimal)await _currencyServices.GetPriceRateExchangeAsync(transactionVM.FromCurrency, otherAccount.Currency);
            var isUsersSelfAccount = await _accountServices.IsAccountForUserAsync(username, int.Parse(transactionVM.OthersAccountIdAsString));
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




        //public async Task<bool> ConfirmTransactionAsync(int transactionId, string userId, bool isConfirm)
        //{
        //    using var safeScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        //    // Validations
        //    var transaction = await _context.Transactions.SingleOrDefaultAsync(x => x.UserId != null && x.TransactionId.Equals(transactionId) && x.UserId.Equals(userId));

        //    if (transaction == null) return false;
        //    if (!transaction.Status.Equals(StatusEnum.Pending)) return false;
        //    var expiredDateTime = transaction.CreatedAt.AddMinutes(10);
        //    if (expiredDateTime < DateTime.UtcNow)
        //    {
        //        await CancelTransactionAsync(transactionId);
        //        safeScope.Complete();
        //        return false;
        //    }
        //    var checkMaxAmountDaily = await CheckDailyTransactionThreshold(userId, transaction.FromAccountId, transaction.Amount, transaction.FromCurrency);
        //    if (!checkMaxAmountDaily)
        //    {
        //        await CancelTransactionAsync(transactionId);
        //        safeScope.Complete();
        //        return false;
        //    }
        //    var userAccount = await _accountServices.GetAccountByIdAsync(transaction.UserId, transaction.FromAccountId);
        //    var toAccount = await _accountServices.GetAccountByIdAsync(transaction.ToAccountId);
        //    if (toAccount == null || userAccount == null)
        //    {
        //        await CancelTransactionAsync(transactionId);
        //        safeScope.Complete();
        //        return false;
        //    }

        //    // processes
        //    if (!toAccount.Currency.Equals(transaction.FromCurrency))
        //    {
        //        toAccount.Balance += await _currencyServices.ConvertCurrencyAsync(userAccount.Currency, toAccount.Currency, transaction.Amount);
        //    }
        //    else
        //    {
        //        toAccount.Balance += transaction.Amount;
        //    }
        //    var totalPriceToPay = transaction.Fee + transaction.Amount;
        //    userAccount.Balance -= totalPriceToPay;
        //    var toAccountUpdate = await _accountServices.UpdateAccountBalanceAsync(_mapper.Map<UpdateAccountViewModel>(toAccount));
        //    var userAccountUpdate = await _accountServices.UpdateAccountAsync(_mapper.Map<UpdateAccountViewModel>(userAccount), transaction.UserId);
        //    if (userAccountUpdate == 0 || toAccountUpdate == 0) return false;
        //    transaction.Outer = true;
        //    transaction.Status = StatusEnum.Completed;
        //    transaction.CompletedAt = DateTime.UtcNow;
        //    transaction.DeductedAmount = totalPriceToPay;
        //    transaction.UserBalance = userAccount.Balance;
        //    _context.Transactions.Update(transaction);
        //    var result = await _context.SaveChangesAsync() > 0;
        //    if (result == false) return false;
        //    safeScope.Complete();
        //    return true;

        //}


        public async Task<ResultDto> ConfirmTransactionAsync(int transactionId, string userId)
        {
            var res = new ResultDto();
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            #region transaction validation
            var transaction = await _context.Transactions.SingleOrDefaultAsync(x => x.UserId != null
                                           && x.TransactionId.Equals(transactionId)
                                           && x.UserId.Equals(userId));
            if (transaction == null)
            {
                res.Message = "There is no transaction with this Id in DB!";
                return res;
            }

            if (!transaction.Status.Equals(StatusEnum.Pending))
            {
                res.Message = "Transaction status is not valid for confirm!";
                return res;
            }

            if (transaction.CreatedAt.AddMinutes(10) < DateTime.UtcNow)
            {
                res.Message = "Transaction has been expired";
                return res;
            }

            var isExceededFromThreshold = await CheckDailyTransactionThreshold(userId, transaction.FromAccountId, transaction.Amount, transaction.FromCurrency);
            if (!isExceededFromThreshold)
            {
                res.Message = "Transaction amount exceeded from daily threshold";
                return res;
            }

            var sourceAccount = await _accountServices.GetAccountByIdAsync(transaction.UserId, transaction.FromAccountId);
            var destinationAccount = await _accountServices.GetAccountByIdAsync(transaction.ToAccountId);

            if (destinationAccount == null || sourceAccount == null)
            {
                res.Message = "Destination Account or Source Account is invalid!";
                return res;
            }

            #endregion

            // handle request
            if (!destinationAccount.Currency.Equals(sourceAccount.Currency))
            {
                var convertedAmount = await _currencyServices.ConvertCurrencyAsync(sourceAccount.Currency,
                    destinationAccount.Currency, transaction.Amount);
                destinationAccount.Balance += convertedAmount;
            }
            else
            {
                destinationAccount.Balance += transaction.Amount;
            }

            var totalAmountPlusFee = transaction.Fee + transaction.Amount;
            sourceAccount.Balance -= totalAmountPlusFee;

            var updateDestinationBalanceRes = await _accountServices.UpdateAccountBalanceAsync(_mapper.Map<UpdateAccountViewModel>(destinationAccount));
            var updateSourceBalanceRes = await _accountServices.UpdateAccountAsync(_mapper.Map<UpdateAccountViewModel>(sourceAccount), transaction.UserId);

            if (updateSourceBalanceRes == 0 || updateDestinationBalanceRes == 0)
            {
                res.Message = "No records were affected! (Transaction has been rollback!)";
                return res;
            }

            // confirm status of transaction
            transaction.Outer = true;
            transaction.Status = StatusEnum.Completed;
            transaction.CompletedAt = DateTime.UtcNow;
            transaction.DeductedAmount = totalAmountPlusFee;
            transaction.UserBalance = sourceAccount.Balance;
            _context.Transactions.Update(transaction);

            if (await _context.SaveChangesAsync() <= 0)
            {
                res.Message = "Failed to save changes to the database. No records were affected.";
                return res;
            }
            transactionScope.Complete();
            res.IsSucceeded = true;
            return res;

        }

        public async Task<List<Transaction>> CanceledPendingTransactionsByTimePassAsync(int min)
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

        public async Task<bool> CheckDailyTransactionThreshold(string userId, int sourceAccountId, decimal transactionAmount, string transactionCurrency)
        {
            var transactions = await _context.Transactions.Where(x =>
                x.CompletedAt != null && x.UserId != null && x.Status == StatusEnum.Completed
                && x.CompletedAt.Value.Date.Equals(DateTime.UtcNow.Date)
                && x.UserId.Equals(userId)
                && x.FromAccountId.Equals(sourceAccountId)
                && x.Outer).ToListAsync();
            decimal dollarBalance = 0;
            foreach (var transaction in transactions)
            {
                // increase & withdraw
                if (transaction.ToAccountId.Equals(sourceAccountId)) continue;
                // Transform
                if (transaction.FromCurrency.Equals(BusinessConstants.BaseCurrency))
                {
                    dollarBalance += transaction.Amount;
                }
                else
                {
                    dollarBalance += await _currencyServices.ConvertCurrencyAsync(transaction.FromCurrency, BusinessConstants.BaseCurrency, transaction.Amount);
                }
            }
            var dollarPrice = await _currencyServices.ConvertCurrencyAsync(transactionCurrency, BusinessConstants.BaseCurrency, transactionAmount);
            return dollarBalance + dollarPrice <= BusinessConstants.MaxTransaction;
        }

        public async Task<string> GetNameAccountForTransaction(int accountId)
        {
            var account = await _context.Accounts.SingleOrDefaultAsync(x => x.AccountId.Equals(accountId));
            return account != null ? $"{account.AccountName} - {account.CartNumber} " : string.Empty ;
        }

        public async Task<string> GetOtherNameAccountForTransaction(int accountId)
        {
            var account = await _context.OthersAccounts.SingleOrDefaultAsync(x => x.AccountId.Equals(accountId));
            return account != null ? $"{account.AccountName} - {account.CartNumber} " : string.Empty;
        }
    }
}
