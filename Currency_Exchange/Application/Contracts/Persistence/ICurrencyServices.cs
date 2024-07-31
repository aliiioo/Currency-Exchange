using System.Dynamic;
using Application.Dtos.CurrencyDtos;
using Domain.Entities;
using System.Collections.Generic;
using Application.Dtos.ErrorsDtos;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public Task<decimal> GetPriceRateExchangeAsync(string fromCurrency, string toCurrency);
        public Task<ExchangeRate> GetRateExchangeAsync(string fromCurrency, string toCurrency);
        public Task<List<CurrencyDtoShow>> GetListCurrencyAsync();
        public Task<List<CurrencyDtoShow>> GetListCurrencyWithoutBase(string currencyCode);
        public Task<CurrencyDetailDto> GetCurrencyDetailAsync(int currencyId);
        public Task<List<CurrencyExchangeFees>> GetCurrencyExchangeFeeAsync(int currencyId);
        public Task<List<CurrencyTransformFees>> GetCurrencyTransformFeeAsync(int currencyId);
        public Task<List<ExchangeRate>> GetCurrencyRatesAsync(int currencyId);
        public Task<UpdateRateDtos> GetCurrencyRateByIdAsync(int rateId);
        public List<SelectListItem> GetSelectListItemsCurrency();

        public Task<bool> IsExchangePriceAccept(int currencyId, decimal price);
        public Task<bool> IsTransformPriceAccept(CreateFeeDtos createFeeDto);


        public Task<bool> IsExistCurrencyByCodeAsync(string codeCurrency);
        public Task<bool> IsExistCurrencyByCodeAsync(string firstCodeCurrency, string secondCodeCurrency);
        public Task<decimal> ConvertCurrencyAsync(string fromCurrency, string toCurrency, decimal amount);
        public Task<ResultDto> CreateCurrencyAsync(CurrencyDto  currencyVM);


        //Fee's
        public Task<int> CreateTransformFeeToCurrencyAsync(CreateFeeDtos Model);
        public Task<bool> UpdateTransformFeeToCurrencyAsync(int feeId, decimal feePrice);
        public Task<int> CreateExchangeFeeToCurrencyAsync(CreateFeeDtos Model);
        public Task<bool> UpdateExchangeFeeToCurrencyAsync(int feeId, decimal feePrice);
        public Task<bool> DeleteExchangeFeeToCurrencyAsync(int feeId, int currencyId);
        public Task<bool> DeleteTransformFeeToCurrencyAsync(int feeId, int currencyId);

        // Rate's
        public Task<int> CreateExchangeRateCurrencyAsync(RateDtos rateVM);
        public Task<bool> UpdateExchangeRateToCurrencyAsync(UpdateRateDtos rateVM);
        public Task<bool> DeleteExchangeRateCurrencyAsync(int rateId);


    }
}
