using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Contracts.Persistence
{
    public interface ICurrencyServices
    {
        public Task<bool> IsCurrencyByCodeAsync(string codeCurrency);
        public Task<bool> IsCurrencyByCodeAsync(string firstCodeCurrency, string secondCodeCurrency);
        public Task<decimal> CurrencyConvertor(string fromCurrency, string toCurrency, decimal amount);

        public Task<decimal> GetRateConversions(string fromCurrency, string toCurrency);
    }
}
