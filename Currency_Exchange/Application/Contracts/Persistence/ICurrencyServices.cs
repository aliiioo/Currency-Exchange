using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.CurrencyDtos;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Application.Contracts.Persistence
{
    public interface ICurrencyServices
    {
        public Task<bool> IsCurrencyByCodeAsync(string codeCurrency);
        public Task<bool> IsCurrencyByCodeAsync(string firstCodeCurrency, string secondCodeCurrency);
        public Task<decimal> CurrencyConvertor(string fromCurrency, string toCurrency, decimal amount);
        public Task<decimal> GetRateConversions(string fromCurrency, string toCurrency);

        public Task<int> CreateCurrency(CreateCurrencyDto  currencyVM);

        public Task CreateTransformFeeToCurrency(int currencyId,decimal amount);
        public Task UpdateTransformFeeToCurrency(int currencyId, decimal amount);
        public Task CreateExchangeFeeToCurrency(int currencyId,decimal amount);
        public Task UpdateExchangeFeeToCurrency(int currencyId, decimal amount);


        public Task CreateExchangeRateCurrency(string currency1, string currency2, decimal amount);
        public Task UpdateExchangeFeeToCurrency(string currency1, string currency2, decimal amount);




    }
}
