using Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Persistence
{
    public class CurrencyServices: ICurrencyServices
    {
        private readonly CurrencyDbContext _context;

        public CurrencyServices(CurrencyDbContext context)
        {
            _context = context;
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

    }
}
