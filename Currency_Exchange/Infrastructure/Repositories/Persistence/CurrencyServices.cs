using Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.CurrencyDtos;
using Domain.Entities;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Infrastructure.Repositories.Persistence
{
    public class CurrencyServices: ICurrencyServices
    {
        private readonly CurrencyDbContext _context;
        private readonly IMapper _mapper;

        public CurrencyServices(CurrencyDbContext currencyDbContext, IMapper mapper)
        {
            _context=currencyDbContext;
            _mapper=mapper;
        }

        public async Task<bool> IsCurrencyByCodeAsync(string codeCurrency)
        {
            var currency = await _context.Currencies.AnyAsync(x => x.CurrencyCode.Equals(codeCurrency));
            return currency;

        }

        public async Task<bool> IsCurrencyByCodeAsync(string firstCodeCurrency, string secondCodeCurrency)
        {
            var currency = await _context.Currencies.AnyAsync(x => x.CurrencyCode.Equals(firstCodeCurrency)&&x.CurrencyCode.Equals(secondCodeCurrency));
            return currency;
        }
    

        public async Task<decimal> CurrencyConvertor(string fromCurrency, string toCurrency, decimal amount)
        {
            var rate =await GetRateConversions(fromCurrency,toCurrency);
            return rate != 0 ? amount * rate : 0;

        }

        public async Task<decimal> GetRateConversions(string fromCurrency, string toCurrency)
        {
            var rate = await _context.CurrencyConversions
                .FirstOrDefaultAsync(x => x.FromCurrency.Equals(fromCurrency) && x.ToCurrency.Equals(toCurrency));
            return rate != null ? rate.Rate : 0;
        }

        public async Task<int> CreateCurrency(CreateCurrencyDto currencyVM)
        {
            var isExistCurrency = await IsCurrencyByCodeAsync(currencyVM.CurrencyCode);
            if (isExistCurrency == false) return 0;
            var Currency= 

            
        }

        public Task CreateTransformFeeToCurrency(int currencyId, decimal amount)
        {
            throw new NotImplementedException();
        }

        public Task UpdateTransformFeeToCurrency(int currencyId, decimal amount)
        {
            throw new NotImplementedException();
        }

        public Task CreateExchangeFeeToCurrency(int currencyId, decimal amount)
        {
            throw new NotImplementedException();
        }

        public Task UpdateExchangeFeeToCurrency(int currencyId, decimal amount)
        {
            throw new NotImplementedException();
        }

        public Task CreateExchangeRateCurrency(string currency1, string currency2, decimal amount)
        {
            throw new NotImplementedException();
        }

        public Task UpdateExchangeFeeToCurrency(string currency1, string currency2, decimal amount)
        {
            throw new NotImplementedException();
        }
    }
}
