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

        public async Task<List<Transaction>> GetListTransactionsForAdmin()
        {
            return await _context.Transactions.ToListAsync();
        }

        public async Task<bool> TransformCurrency(CreateTransactionDtos transactionVM, string username)
        {
            var isSelfAccount = await _accountServices.IsAccountForUser(username, transactionVM.SelfAccountId);
            if (!isSelfAccount) return false;
            var isOtherAccount=await _othersAccountServices.IsAccountForOthers(username,transactionVM.OthersAccountId);
            if (!isOtherAccount) return false;

            var transaction = _mapper.Map<Transaction>(transactionVM);
            transaction.Status = StatusEnum.Pending;

            transaction.ExchangeRate = await _currencyServices.GetPriceRateExchange(transactionVM.FromCurrency, transactionVM.ToCurrency);
            transaction.Fee = await _currencyServices.GetTransformFeeCurrencyAsync(transactionVM.FromCurrency, transactionVM.ToCurrency, transactionVM.Amount);

            var totalPriceToPay = transaction.ExchangeRate + transaction.Fee + transaction.Amount;
            var account = await _accountServices.GetAccountByIdAsync(transactionVM.Username, transactionVM.SelfAccountId);
            var otherAccount = await _othersAccountServices.GetOtherAccountByIdAsync(transactionVM.OthersAccountId, username);
            var convertBalance = new decimal();
            if (!transactionVM.ToCurrency.Equals(transactionVM.FromCurrency))
            {
                convertBalance = await _currencyServices.CurrencyConvertor(transactionVM.FromCurrency, transactionVM.ToCurrency, transactionVM.Amount);
            }
            else
            {
                convertBalance = transactionVM.Amount;
            }
            account.Balance = account.Balance - totalPriceToPay;
            otherAccount.Balance = convertBalance;
            await _accountServices.UpdateAccount(_mapper.Map<UpdateAccountViewModel>(account));
            await _othersAccountServices.UpdateOthersAccountAsync(_mapper.Map<UpdateOtherAccountViewModel>(otherAccount));
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> TransformToSelfAccountCurrency(CreateTransactionDtos transactionVM, string username)
        {

            var isSelfAccount = await _accountServices.IsAccountForUser(username, transactionVM.SelfAccountId);
            if (!isSelfAccount) return false;
            
            var transaction = _mapper.Map<Transaction>(transactionVM);
            transaction.Status = StatusEnum.Pending;
            transaction.ExchangeRate = await _currencyServices.GetPriceRateExchange(transactionVM.FromCurrency, transactionVM.ToCurrency);
            var totalPriceToPay = transaction.ExchangeRate + transaction.Amount;
            var account = await _accountServices.GetAccountByIdAsync(transactionVM.Username, transactionVM.SelfAccountId);
            var selfAccount = await _accountServices.GetAccountByIdAsync(username,transactionVM.OthersAccountId);
            var convertBalance = new decimal();
            if (!transactionVM.ToCurrency.Equals(transactionVM.FromCurrency))
            {
                convertBalance = await _currencyServices.CurrencyConvertor(transactionVM.FromCurrency, transactionVM.ToCurrency, transactionVM.Amount);
            }
            else
            {
                convertBalance = transactionVM.Amount;
            }
            account.Balance = account.Balance - totalPriceToPay;
            selfAccount.Balance = convertBalance;
            await _accountServices.UpdateAccount(_mapper.Map<UpdateAccountViewModel>(account));
            await _accountServices.UpdateAccount(_mapper.Map<UpdateAccountViewModel>(selfAccount));
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return true;


        }

        public async Task<bool> ConfirmTransaction(int transactionId, string username, bool isConfirm)
        {
            var transaction= await _context.Transactions.SingleOrDefaultAsync(x =>
                x.TransactionId.Equals(transactionId) && x.FromAccountId.Equals(username));
            if (transaction == null) return false;
            var expiredDateTime = transaction.CreatedAt.AddMinutes(10);
            if (expiredDateTime<DateTime.UtcNow)
            {
                return false;
            }
            if (isConfirm)
            {
                transaction.Status = StatusEnum.Completed;
            }
            transaction.Status = StatusEnum.Cancelled;
            transaction.CompletedAt = DateTime.UtcNow;
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
