using Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.DbContexts;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Application.Dtos.TransactionDtos;
using Application.Statics;
using Application.Dtos.AccountDtos;

namespace Infrastructure.Repositories.Persistence
{
    public class ProviderServices: IProviderServices
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

        public async Task<Transaction> GetTransaction(int idTransaction)
        {
            return await _context.Transactions.SingleOrDefaultAsync(x => x.TransactionId.Equals(idTransaction));
        }

        public async Task<List<Transaction>> GetListTransactions(int fromIdAccount)
        {
            return await _context.Transactions.Where(x => x.FromAccountId.Equals(fromIdAccount)).ToListAsync();
        }

        public async Task<List<Transaction>> GetListTransactionsForAdmin()
        {
            return await _context.Transactions.ToListAsync();
        }

        public async Task<bool> TransformCurrency(CreateTransactionDtos transactionVM)
        {
            var transaction = _mapper.Map<Transaction>(transactionVM);
            transaction.Status = StatusEnum.Pending;
            transaction.ExchangeRate=await _currencyServices.GetPriceRateExchange(transactionVM.FromCurrency,transactionVM.ToCurrency);
            transaction.Fee=await _currencyServices.GetTransformFeeCurrencyAsync(transactionVM.FromCurrency,transactionVM.ToCurrency,transactionVM.Amount);
            var totalPriceToPay = transaction.ExchangeRate + transaction.Fee + transaction.Amount;
            var account = await _accountServices.GetAccountByIdAsync(transactionVM.Username,transactionVM.SelfAccountId);
            account.Balance = account.Balance - totalPriceToPay;
            await _accountServices.UpdateAccount(_mapper.Map<UpdateAccountViewModel>(account));

            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
