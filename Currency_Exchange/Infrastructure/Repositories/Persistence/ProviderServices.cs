using Application.Contracts.Persistence;
using Application.Dtos.AccountDtos;
using Application.Dtos.OthersAccountDto;
using Application.Dtos.TransactionDtos;
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
            var account = await _accountServices.GetAccountByIdAsync(transactionVM.Username, transactionVM.SelfAccountId);
            var otherAccount = await _othersAccountServices.GetOtherAccountByIdAsync(transactionVM.OthersAccountId, username);

            var transaction = await CreateTransaction(transactionVM);
            transaction.ExchangeRate = await _currencyServices.GetPriceRateExchange(transactionVM.FromCurrency, otherAccount.Currency);
            transaction.Fee = await _currencyServices.GetTransformFeeCurrencyAsync(transactionVM.FromCurrency, otherAccount.Currency, transactionVM.Amount);
            transaction.Fee += await _currencyServices.GetExchangeFeeCurrencyAsync(transactionVM.FromCurrency, otherAccount.Currency, transactionVM.Amount);
            transaction.ToCurrency = otherAccount.Currency;
            transaction.ToOtherAccountId = transactionVM.OthersAccountId;

            var totalPriceToPay = transaction.ExchangeRate + transaction.Fee + transaction.Amount;
            account.Balance -= totalPriceToPay;
            if (!otherAccount.Currency.Equals(transactionVM.FromCurrency))
            {
               
                otherAccount.Balance += await _currencyServices.CurrencyConvertor(transactionVM.FromCurrency, otherAccount.Currency, transactionVM.Amount);
            }
            else
            {
                otherAccount.Balance += transactionVM.Amount;
            }
            var accountUpdate = await _accountServices.UpdateAccount(_mapper.Map<UpdateAccountViewModel>(account), transactionVM.Username);
            if (accountUpdate == 0) return 0;
            var otherAccountUpdate = await _othersAccountServices.UpdateOthersAccountAsync(_mapper.Map<UpdateOtherAccountViewModel>(otherAccount), transactionVM.Username);
            if (otherAccountUpdate == 0) return 0;
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return transaction.TransactionId;
        }

        public async Task<Transaction> CreateTransaction(CreateTransactionDtos transactionVM)
        {
            var transaction = _mapper.Map<Transaction>(transactionVM);
            transaction.Status = StatusEnum.Pending;

            return transaction;
        }

        public async Task<int> TransformToSelfAccountCurrency(CreateTransactionDtos transactionVM, string username)
        {
            var isSelfAccount = await _accountServices.IsAccountForUser(username, transactionVM.SelfAccountId);
            if (!isSelfAccount) return 0;
            var account = await _accountServices.GetAccountByIdAsync(transactionVM.Username, transactionVM.SelfAccountId);
            var otherSelfAccount = await _accountServices.GetAccountByIdAsync(username, transactionVM.OthersAccountId);

            var transaction = await CreateTransaction(transactionVM);
            transaction.ToAccountId = transactionVM.OthersAccountId;
            transaction.ToCurrency = otherSelfAccount.Currency;
            transaction.ExchangeRate = await _currencyServices.GetPriceRateExchange(transactionVM.FromCurrency, otherSelfAccount.Currency);

            account.Balance -= transaction.ExchangeRate + transaction.Amount;
            if (!otherSelfAccount.Currency.Equals(transactionVM.FromCurrency))
            {
                otherSelfAccount.Balance += await _currencyServices.CurrencyConvertor(transactionVM.FromCurrency, otherSelfAccount.Currency, transactionVM.Amount);
            }
            else
            {
                otherSelfAccount.Balance += transactionVM.Amount;
            }

            var accountUpdate = await _accountServices.UpdateAccount(_mapper.Map<UpdateAccountViewModel>(account), transactionVM.Username);
            if (accountUpdate == 0) return 0;
            var otherAccountUpdate = await _accountServices.UpdateAccount(_mapper.Map<UpdateAccountViewModel>(otherSelfAccount), transactionVM.Username);
            if (otherAccountUpdate == 0) return 0;
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return transaction.TransactionId;


        }

        public async Task<bool> ConfirmTransaction(int transactionId, string username, bool isConfirm)
        {
            var transaction = await _context.Transactions.SingleOrDefaultAsync(x =>
                x.TransactionId.Equals(transactionId) && x.UserId.Equals(username));
            if (transaction == null) return false;
            var expiredDateTime = transaction.CreatedAt.AddMinutes(10);
            if (expiredDateTime < DateTime.UtcNow)
            {
                return false;
            }
            transaction.Status = isConfirm ? StatusEnum.Completed : StatusEnum.Cancelled;
            transaction.CompletedAt = DateTime.UtcNow;
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Transaction>> CanceledPendingTransactionsByTimePass(int min)
        {
            var timer = DateTime.UtcNow.AddMinutes(-min);
            var recentTransactions = await _context.Transactions
                .Where(t => t.CreatedAt >= timer&&t.Status.Equals(StatusEnum.Pending))
                .ToListAsync();
            foreach (var item in recentTransactions)
            {
                item.Status=StatusEnum.Cancelled;
            }
            _context.Transactions.UpdateRange(recentTransactions);
            await _context.SaveChangesAsync();
            return recentTransactions;
        }
    }
}
