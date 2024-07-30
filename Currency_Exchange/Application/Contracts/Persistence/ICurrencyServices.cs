using System.Dynamic;
using Application.Dtos.CurrencyDtos;
using Domain.Entities;
using System.Collections.Generic;

namespace Application.Contracts.Persistence
{
    public interface ICurrencyServices
    {
        // Get
        public Task<Currency?> GetCurrencyByIdAsync(int currencyId);
        public Task<Currency?> GetCurrencyByCodeAsync(string currencyCode);
        public Task<UpdateFeeDtos?> GetExchangeFeeCurrencyByIdAsync(int id);
        public Task<List<CurrencyExchangeFees>> GetListExchangeFeesAsync(string fromCurrency ,string toCurrency);
        public Task<UpdateFeeDtos?> GetTransformFeeCurrencyByIdAsync(int id);
        public Task<decimal> GetTransformFeeCurrencyAsync(string fromCurrency, string toCurrency,decimal price);
        public Task<decimal> GetExchangeFeeCurrencyAsync(string fromCurrency, string toCurrency,decimal price);
        public Task<List<CurrencyTransformFees>> GetListTransformFeesAsync(string fromCurrency, string toCurrency);
        public Task<decimal> GetPriceRateExchange(string fromCurrency, string toCurrency);
        public Task<ExchangeRate> GetRateExchange(string fromCurrency, string toCurrency);
        public Task<List<CurrencyDtoShow>> GetListCurrency();
        public Task<List<CurrencyDtoShow>> GetListCurrencyWithoutBase(string currencyCode);
        public Task<CurrencyDetailDto> GetCurrencyDetail(int currencyId);
        public Task<List<CurrencyExchangeFees>> GetCurrencyExchangeFeeAsync(int currencyId);
        public Task<List<CurrencyTransformFees>> GetCurrencyTransformFeeAsync(int currencyId);
        public Task<List<ExchangeRate>> GetCurrencyRatesAsync(int currencyId);
        public Task<UpdateRateDtos> GetCurrencyRateByIdAsync(int rateId);

        public Task<bool> IsExchangePriceAccept(int currencyId, decimal price);
        public Task<bool> IsTransformPriceAccept(CreateFeeDtos createFeeDto);


        public Task<bool> IsExistCurrencyByCodeAsync(string codeCurrency);
        public Task<bool> IsExistCurrencyByCodeAsync(string firstCodeCurrency, string secondCodeCurrency);
        public Task<decimal> CurrencyConvertor(string fromCurrency, string toCurrency, decimal amount);
        public Task<int> CreateCurrency(CurrencyDto  currencyVM);


        //Fee's
        public Task<int> CreateTransformFeeToCurrency(CreateFeeDtos Model);
        public Task<bool> UpdateTransformFeeToCurrency(int feeId, decimal feePrice);
        public Task<int> CreateExchangeFeeToCurrency(CreateFeeDtos Model);
        public Task<bool> UpdateExchangeFeeToCurrency(int feeId, decimal feePrice);
        public Task<bool> DeleteExchangeFeeToCurrency(int feeId, int currencyId);
        public Task<bool> DeleteTransformFeeToCurrency(int feeId, int currencyId);

        // Rate's
        public Task<int> CreateExchangeRateCurrency(RateDtos rateVM);
        public Task<bool> UpdateExchangeRateToCurrency(UpdateRateDtos rateVM);
        public Task<bool> DeleteExchangeRateCurrency(int rateId);


    }
}
