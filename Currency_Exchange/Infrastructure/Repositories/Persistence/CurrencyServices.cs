using System.Security.Cryptography.X509Certificates;
using Application.Contracts;
using Application.Contracts.Persistence;
using Application.Dtos.CurrencyDtos;
using AutoMapper;
using Domain.Entities;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Reflection.Metadata.Ecma335;
using System.Security.AccessControl;
using Microsoft.AspNetCore.Mvc.Rendering;
using Application.API_Calls;
using Application.Dtos.ErrorsDtos;

namespace Infrastructure.Repositories.Persistence
{
    public class CurrencyServices : ICurrencyServices
    {
        private readonly CurrencyDbContext _context;
        private readonly IMapper _mapper;
        private readonly IApiServices _apiServices;

        public CurrencyServices(CurrencyDbContext context, IMapper mapper, IApiServices apiServices)
        {
            _context = context;
            _mapper = mapper;
            _apiServices = apiServices;
        }
        public async Task<CurrencyDetailDto> GetCurrencyDetailAsync(int currencyId)
        {
            var currency = await _context.Currencies.Include(x => x.CurrencyTransformFees)
                .Include(x => x.CurrencyExchangeFees).Include(x => x.ExchangeRate)
                .SingleOrDefaultAsync(x => x.CurrencyId.Equals(currencyId));
            var currencyDto = _mapper.Map<CurrencyDetailDto>(currency);
            return currencyDto;
        }

        public async Task<List<CurrencyExchangeFees>> GetCurrencyExchangeFeeAsync(int currencyId)
        {
            return await _context.CurrencyExchangeFees.Where(x => x.CurrencyId.Equals(currencyId))
                .OrderBy(x => x.ToCurrency).ThenBy(x => x.EndRange).ToListAsync();
        }

        public async Task<List<CurrencyTransformFees>> GetCurrencyTransformFeeAsync(int currencyId)
        {
            return await _context.CurrencyTransformFees.Where(x => x.CurrencyId.Equals(currencyId))
                .OrderBy(x => x.ToCurrency).ThenBy(x => x.EndRange).ToListAsync();
        }

        public async Task<List<ExchangeRate>> GetCurrencyRatesAsync(int currencyId)
        {
            return await _context.ExchangeRates.Where(x => x.CurrencyId.Equals(currencyId)).ToListAsync();
        }

        public async Task<UpdateRateDtos> GetCurrencyRateByIdAsync(int rateId)
        {
            var rate = await _context.ExchangeRates.SingleOrDefaultAsync(x => x.ExchangeRateId == rateId);
            return _mapper.Map<UpdateRateDtos>(rate);
        }



        public async Task<bool> IsExchangePriceAccept(int currencyId, decimal price)
        {
            var lastPriceAccept = await _context.CurrencyExchangeFees.Where(x => x.CurrencyId.Equals(currencyId))
                .OrderBy(x => x.ToCurrency).ThenBy(x => x.EndRange).LastOrDefaultAsync();
            if (lastPriceAccept == null) return true;
            return lastPriceAccept.PriceFee >= price;
        }

        public async Task<bool> IsTransformPriceAccept(CreateFeeDtos createFeeDto)
        {
            var lastPriceAccept = await _context.CurrencyTransformFees.Where(x => x.CurrencyId.Equals(createFeeDto.CurrencyId)
                    && x.FromCurrency.Equals(createFeeDto.FromCurrency) && x.ToCurrency.Equals(createFeeDto.ToCurrency))
                .OrderBy(x => x.ToCurrency).ThenBy(x => x.EndRange).LastOrDefaultAsync();
            if (lastPriceAccept == null) return true;
            return lastPriceAccept.PriceFee >= createFeeDto.PriceFee;
        }

        public async Task<List<CurrencyDtoShow>> GetListCurrencyAsync()
        {
            return _mapper.Map<List<CurrencyDtoShow>>(await _context.Currencies.ToListAsync());
        }

        public async Task<List<CurrencyDtoShow>> GetListCurrencyWithoutBase(string currencyCode)
        {
            return _mapper.Map<List<CurrencyDtoShow>>(await _context.Currencies.Where(x => x != null && !x.CurrencyCode.Equals(currencyCode)).ToListAsync());
        }

        public async Task<Currency?> GetCurrencyByIdAsync(int currencyId)
        {
            return await _context.Currencies.SingleOrDefaultAsync(x => x.CurrencyId.Equals(currencyId));
        }

        public async Task<Currency?> GetCurrencyByCodeAsync(string currencyCode)
        {
            return await _context.Currencies.SingleOrDefaultAsync(x => x.CurrencyCode.Equals(currencyCode));
        }

        public async Task<UpdateFeeDtos?> GetExchangeFeeCurrencyByIdAsync(int id)
        {
            var fee = await _context.CurrencyExchangeFees.SingleOrDefaultAsync(x => x.FeeId.Equals(id));
            return _mapper.Map<UpdateFeeDtos>(fee);
        }

        public async Task<List<CurrencyExchangeFees>> GetListExchangeFeesAsync(string fromCurrency, string toCurrency)
        {
            return await _context.CurrencyExchangeFees.Where(x =>
                x.FromCurrency.Equals(fromCurrency) && x.ToCurrency.Equals(toCurrency)).ToListAsync();
        }

        public async Task<decimal> GetTransformFeeCurrencyAsync(string fromCurrency, string toCurrency, decimal price)
        {
            var priceFee = await _context.CurrencyTransformFees.SingleOrDefaultAsync(x =>
                x.FromCurrency.Equals(fromCurrency) && x.ToCurrency.Equals(toCurrency) && x.StartRange < price && x.EndRange >= price);
            return priceFee != null ? (price * priceFee.PriceFee) / 100 : 0;
        }

        public async Task<decimal> GetExchangeFeeCurrencyAsync(string fromCurrency, string toCurrency, decimal price)
        {
            //validate
            if (fromCurrency.Equals(toCurrency)) return 0;
            // processes
            var priceFee = await _context.CurrencyExchangeFees.SingleOrDefaultAsync(x =>
                x.FromCurrency.Equals(fromCurrency) && x.ToCurrency.Equals(toCurrency) && x.StartRange < price && x.EndRange >= price);
            return priceFee?.PriceFee * price / 100 ?? 0;
        }

        public async Task<UpdateFeeDtos?> GetTransformFeeCurrencyByIdAsync(int id)
        {
            var fees = await _context.CurrencyTransformFees.SingleOrDefaultAsync(x => x.FeeId.Equals(id));
            return _mapper.Map<UpdateFeeDtos>(fees);
        }

        public async Task<List<CurrencyTransformFees>> GetListTransformFeesAsync(string fromCurrency, string toCurrency)
        {
            return await _context.CurrencyTransformFees.Where(x =>
                x.FromCurrency.Equals(fromCurrency) && x.ToCurrency.Equals(toCurrency)).ToListAsync();
        }


        public async Task<bool> IsExistCurrencyByCodeAsync(string codeCurrency)
        {

            return await _context.Currencies.AnyAsync(x => x.CurrencyCode.Equals(codeCurrency.ToUpper()));
        }

        public async Task<bool> IsExistCurrencyByCodeAsync(string firstCodeCurrency, string secondCodeCurrency)
        {
            return await _context.Currencies.AnyAsync(x => x.CurrencyCode.Equals(firstCodeCurrency) && x.CurrencyCode.Equals(secondCodeCurrency));
        }


        public async Task<decimal> ConvertCurrencyAsync(string fromCurrency, string toCurrency, decimal amount)
        {
            var rate = await GetPriceRateExchangeAsync(fromCurrency, toCurrency);
            return amount * rate;
        }

        public async Task<decimal> GetPriceRateExchangeAsync(string fromCurrency, string toCurrency)
        {
            //validate
            if (fromCurrency.Equals(toCurrency)) return 1;
            // processes
            var rate = await _context.ExchangeRates
            .FirstOrDefaultAsync(x => x.FromCurrency.Equals(fromCurrency) && x.ToCurrency.Equals(toCurrency));
            if (rate == null)
            {
                var apiResult= await _apiServices.GetExchangeRateAsync(fromCurrency, toCurrency);
                return apiResult!=100 ? apiResult : 1;
            }
            return rate.Rate;
        }

        public async Task<ExchangeRate> GetRateExchangeAsync(string fromCurrency, string toCurrency)
        {
            var rate = await _context.ExchangeRates
                .FirstOrDefaultAsync(x => x.FromCurrency.Equals(fromCurrency) && x.ToCurrency.Equals(toCurrency));
            return rate != null ? rate : null;
        }

        private async Task<bool> IsCurrencyGlobalAsync(string currencyCode)
        {
            var result=await _apiServices.GetExchangeRateAsync(currencyCode, "USD");
            return result != 100;
        }

        public async Task<ResultDto> CreateCurrencyAsync(CurrencyDto currencyVM)
        {
            var result = new ResultDto();
            try
            {
                // validate input
                if (currencyVM.CurrencyCode.Length is > 4 or < 3)
                {
                    result.Message = "Currency Code Must between 3 to 4 Char";
                    return result;
                }
                if (!await IsCurrencyGlobalAsync(currencyVM.CurrencyCode))
                {
                    result.Message = "It's Not global Currency";
                    return result;
                }

                // validate with DB
                if (await IsExistCurrencyByCodeAsync(currencyVM.CurrencyCode))
                {
                    result.Message = "Currency is Exist";
                    return result;
                }
                
                // handle request
                currencyVM.CurrencyCode = currencyVM.CurrencyCode.ToUpper();
                var currency = _mapper.Map<Currency>(currencyVM);

                await _context.Currencies.AddAsync(currency);
                if (await _context.SaveChangesAsync() <= 0)
                {
                    result.Message = "Error in Db Operation!";
                    return result;
                }

                result.IsSucceeded = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Message = $"Operation has been failed : {ex.Message}";
                return result;
            }

        }


        public async Task<bool> DeleteExchangeFeeToCurrencyAsync(int feeId, int currencyId)
        {
            var fee = await _context.CurrencyExchangeFees.SingleOrDefaultAsync(x => x.FeeId.Equals(feeId));
            if (fee == null) return false;
            //validate
            var feesList = await GetCurrencyExchangeFeeAsync(currencyId);
            var currentIndex = feesList.IndexOf(fee);
            var nextItem = currentIndex < feesList.Count - 1 ? feesList[currentIndex + 1] : null;
            if (nextItem != null)
            {
                // first or middle item  
                nextItem.StartRange = fee.StartRange;
                _context.CurrencyExchangeFees.Update(nextItem);
            }
            _context.CurrencyExchangeFees.Remove(fee);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteTransformFeeToCurrencyAsync(int feeId, int currencyId)
        {
            var fee = await _context.CurrencyTransformFees.SingleOrDefaultAsync(x => x.FeeId.Equals(feeId));
            if (fee == null) return false;
            //validate
            var feesList = await GetCurrencyTransformFeeAsync(currencyId);
            var currentIndex = feesList.IndexOf(fee);
            var nextItem = currentIndex < feesList.Count - 1 ? feesList[currentIndex + 1] : null;
            if (nextItem != null)
            {
                // first or middle item  
                nextItem.StartRange = fee.StartRange;
                _context.CurrencyTransformFees.Update(nextItem);
            }
            // processes
            _context.CurrencyTransformFees.Remove(fee);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> CreateExchangeRateCurrencyAsync(RateDtos rateVM)
        {
            //validate
            if (rateVM.Rate is < 0 or > 40) return 0;
            if (rateVM.FromCurrency.Equals(rateVM.ToCurrency)) return 1;
            var existRate = await _context.ExchangeRates.FirstOrDefaultAsync(x => x.FromCurrency.Equals(rateVM.FromCurrency) && x.ToCurrency.Equals(rateVM.ToCurrency));
            if (existRate != null) return 0;
            // processes
            var rate = _mapper.Map<ExchangeRate>(rateVM);
            await _context.ExchangeRates.AddAsync(rate);
            var queryResult = await _context.SaveChangesAsync();
            return queryResult > 0 ? rate.ExchangeRateId : 0;
        }

        public async Task<bool> UpdateExchangeRateToCurrencyAsync(UpdateRateDtos rateVM)
        {
            //validate
            if (rateVM.Rate is < 0 or > 40) return false;
            var existRate = await _context.ExchangeRates.SingleOrDefaultAsync(x => x.ExchangeRateId.Equals(rateVM.ExchangeRateId));
            if (existRate == null) return false;
            // processes
            existRate.Rate = rateVM.Rate;
            _context.ExchangeRates.Update(existRate);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteExchangeRateCurrencyAsync(int rateId)
        {
            //validate
            var existRate = await _context.ExchangeRates.SingleOrDefaultAsync(x => x.ExchangeRateId.Equals(rateId));
            if (existRate == null) return false;
            // processes
            _context.ExchangeRates.Remove(existRate);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> CreateTransformFeeToCurrencyAsync(CreateFeeDtos Model)
        {
            //validate
            if (Model.PriceFee is < 0 or > 40) return 0;
            var isPriceExcept = await IsTransformPriceAccept(Model);
            if (isPriceExcept == false) return 0;
            var lastOldFee = GetListTransformFeesAsync(Model.FromCurrency, Model.ToCurrency).Result.LastOrDefault();
            var fee = _mapper.Map<CurrencyTransformFees>(Model);
            if (lastOldFee != null)
            {
                if (lastOldFee.EndRange + 1 > fee.EndRange)
                {
                    return 0;
                }
                fee.StartRange = lastOldFee.EndRange + 1;
            }
            else
            {
                fee.StartRange = 0;
            }
            // processes
            await _context.CurrencyTransformFees.AddAsync(fee);
            var queryResult = await _context.SaveChangesAsync();
            return queryResult > 0 ? fee.FeeId : 0;
        }

        public async Task<bool> UpdateTransformFeeToCurrencyAsync(int feeId, decimal feePrice)
        {
            //validate
            if (feePrice is < 0 or > 40) return false;
            var fee = await _context.CurrencyTransformFees.SingleOrDefaultAsync(x => x.FeeId.Equals(feeId));
            if (fee == null) return false;
            var feesList = await GetCurrencyTransformFeeAsync(fee.CurrencyId);
            feesList = feesList.Where(x => x.FromCurrency.Equals(fee.FromCurrency) && x.ToCurrency.Equals(fee.ToCurrency)).ToList();
            var currentIndex = feesList.IndexOf(fee);
            var nextItem = currentIndex < feesList.Count - 1 ? feesList[currentIndex + 1] : null;
            var previousItem = currentIndex > 0 ? feesList[currentIndex - 1] : null;
            if (nextItem != null)
            {
                // first or middle item  
                if (nextItem.PriceFee > feePrice)
                {
                    return false;
                }
            }
            if (previousItem != null)
            {
                if (previousItem.PriceFee < feePrice)
                {
                    return false;
                }
            }
            // processes
            fee.PriceFee = feePrice;
            _context.CurrencyTransformFees.Update(fee);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<ResultDto> CreateExchangeFeeToCurrencyAsync(CreateFeeDtos Model)
        {
            var result = new ResultDto();
            //validate
            if (Model.PriceFee is < 0 or > 40)
            {
                result.Message="Price Must be in range 0% to 40% ";
                return result;
            }

            if (Model.ToCurrency.Equals(Model.FromCurrency))
            {
                result.Message = "Don't Have Fee with same Currency ";
                return result;
            }

            var isPriceAccept= await IsExchangePriceAccept(Model.CurrencyId, Model.PriceFee);
            if (isPriceAccept == false)
            {
                result.Message = "Fee Price Must be Between Next And Previous";
                return result;
            }

            var lastOldFee = GetListExchangeFeesAsync(Model.FromCurrency, Model.ToCurrency).Result.LastOrDefault();
            var fee = _mapper.Map<CurrencyExchangeFees>(Model);
            if (lastOldFee != null)
            {
                if (lastOldFee.EndRange + 1 > fee.EndRange)
                {
                    result.Message = "End Range Must Bigger than Previous";
                    return result;
                }
                fee.StartRange = lastOldFee.EndRange + 1;
            }
            else
            {
                fee.StartRange = 0;
            }
            // Handel Request
            await _context.CurrencyExchangeFees.AddAsync(fee);
            await _context.SaveChangesAsync();
            if (await _context.SaveChangesAsync()<=0)
            {
                result.Message = "Exchange dose not SAve Try it again";
                return result;
            }
            result.IsSucceeded = true;
            return result;
        }

        public async Task<bool> UpdateExchangeFeeToCurrencyAsync(int feeId, decimal feePrice)
        {
            //validate
            if (feePrice is < 0 or > 40) return false;
            var fee = await _context.CurrencyExchangeFees.SingleOrDefaultAsync(x => x.FeeId.Equals(feeId));
            if (fee == null) return false;
            var feesList = await GetCurrencyExchangeFeeAsync(fee.CurrencyId);
            feesList = feesList.Where(x => x.FromCurrency.Equals(fee.FromCurrency) && x.ToCurrency.Equals(fee.ToCurrency)).ToList();
            var currentIndex = feesList.IndexOf(fee);
            var nextItem = currentIndex < feesList.Count - 1 ? feesList[currentIndex + 1] : null;
            var previousItem = currentIndex > 0 ? feesList[currentIndex - 1] : null;
            if (nextItem != null)
            {
                // first or middle item  
                if (nextItem.PriceFee > feePrice)
                {
                    return false;
                }
            }
            if (previousItem != null)
            {
                if (previousItem.PriceFee < feePrice)
                {
                    return false;
                }
            }
            // processes
            fee.PriceFee = feePrice;
            _context.CurrencyExchangeFees.Update(fee);
            return await _context.SaveChangesAsync() > 0;
        }

        public List<SelectListItem> GetSelectListItemsCurrency()
        {

            var currency = GetListCurrencyAsync().Result
                .Select(x => new SelectListItem { Value = x.CurrencyCode.ToUpper().ToString(), Text = x.CurrencyCode.ToString() }).ToList();
            currency.Insert(0, new SelectListItem { Value = "", Text = "Choice" });
            return currency;
        }
    }
}
