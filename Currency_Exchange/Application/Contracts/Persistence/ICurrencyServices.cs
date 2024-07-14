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
        public Task<Currency?> GetCurrencyByIdAsync(int currencyId);
        public Task<Currency?> GetCurrencyByCodeAsync(string currencyCode);
        public Task<CurrencyExchangeFees?> GetExchangeFeeCurrencyByIdAsync(int id);
        public Task<List<CurrencyExchangeFees>> GetListExchangeFeesAsync(string fromCurrency ,string toCurrency);
        public Task<CurrencyTransformFees?> GetTransformFeeCurrencyByIdAsync(int id);
        public Task<List<CurrencyTransformFees>> GetListTransformFeesAsync(string fromCurrency, string toCurrency);
        public Task<decimal> GetPriceRateExchange(string fromCurrency, string toCurrency);
        public Task<ExchangeRate> GetRateExchange(string fromCurrency, string toCurrency);

        public Task<bool> IsCurrencyByCodeAsync(string codeCurrency);
        public Task<bool> IsCurrencyByCodeAsync(string firstCodeCurrency, string secondCodeCurrency);

        public Task<decimal> CurrencyConvertor(string fromCurrency, string toCurrency, decimal amount);

        public Task<int> CreateCurrency(CreateCurrencyDto  currencyVM);


        public Task<int> CreateTransformFeeToCurrency(CreateFeeDtos Model);
        public Task<bool> UpdateTransformFeeToCurrency(UpdateFeeDtos Model);

        public Task<int> CreateExchangeFeeToCurrency(CreateFeeDtos Model);
        public Task<bool> UpdateExchangeFeeToCurrency(UpdateFeeDtos Model);


        public Task<int> CreateExchangeRateCurrency(RateDtos rateVM);
        public Task<bool> UpdateExchangeRateToCurrency(RateDtos rateVM);




    }
}
