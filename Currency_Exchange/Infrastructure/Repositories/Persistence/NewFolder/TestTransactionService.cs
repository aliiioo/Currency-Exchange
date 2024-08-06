using Application.Contracts.Persistence;
using Application.Dtos.AccountDtos;
using Application.Dtos.ErrorsDtos;
using AutoMapper;
using Domain.Entities;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Application.Dtos.TransactionDtos;
using Transaction = Domain.Entities.Transaction;

namespace Infrastructure.Repositories.Persistence.NewFolder
{
    public class TestTransactionService: ITestTransaction
    {
        private readonly ICurrencyServices _currencyServices;
        private readonly IAccountServices _accountServices;
        private readonly CurrencyDbContext _context;
        private readonly IMapper _mapper;

        public TestTransactionService(ICurrencyServices currencyServices, IAccountServices accountServices, CurrencyDbContext context, IMapper mapper)
        {
            _currencyServices = currencyServices;
            _accountServices = accountServices;
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultDto> ConfirmTransactionAsyncFalt1(int transactionId, string userId)
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

            return res;

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


        public async Task<ResultDto> ConfirmTransactionAsyncFalt2(int transactionId, string userId)
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


            return res;
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



        public async Task<ResultDto> ConfirmTransactionAsyncFalt3(int transactionId, string userId)
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

            return res;
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


        public async Task<ResultDto> ConfirmTransactionAsyncFalt4(int transactionId, string userId)
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

            return res;

            transactionScope.Complete();
            res.IsSucceeded = true;
            return res;

        }


        public async Task<ResultDto> ConfirmTransactionAsyncFalt5(int transactionId, string userId)
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
            throw new Exception();
            transactionScope.Complete();
            res.IsSucceeded = true;
            return res;

        }


        public async Task<ResultDto> ConfirmTransactionAsyncTrue(int transactionId, string userId)
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

        public async Task<int> TransformCurrencyTest(string username)
        {
            CreateTransactionDtos transactionVM =new CreateTransactionDtos()
            {
                Amount = 1,
                FromCurrency = "USD",
                UserId = username,
                SelfAccountId = 2062,
                OthersAccountId = 2063,
                OthersAccountIdAsString = "2063"
            };
            //validate
            // processes
            var transaction = _mapper.Map<Transaction>(transactionVM);
            transaction.Status = StatusEnum.Pending;

            var isUsersSelfAccount = await _accountServices.IsAccountForUserAsync(username, int.Parse(transactionVM.OthersAccountIdAsString));
            transaction.ToCurrency = "USD";
            transaction.ToAccountId = int.Parse(transactionVM.OthersAccountIdAsString);
            await _context.Transactions.AddAsync(transaction);
            var queryResult = await _context.SaveChangesAsync();
            return queryResult > 0 ? transaction.TransactionId : 0;
        }

    }
}
