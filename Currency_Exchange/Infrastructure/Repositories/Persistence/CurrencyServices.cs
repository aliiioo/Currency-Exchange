﻿using Application.Contracts;
using Application.Contracts.Persistence;
using Application.Dtos.CurrencyDtos;
using AutoMapper;
using Domain.Entities;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

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
        public async Task<CurrencyDetailDto> GetCurrencyDetail(int currencyId)
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

        public async Task<List<CurrencyDtoShow>> GetListCurrency()
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
                x.FromCurrency.Equals(fromCurrency) && x.ToCurrency.Equals(toCurrency)&&x.StartRange<price&&x.EndRange>price);
            return priceFee != null ? (price * priceFee.PriceFee) / 100 : 0;
        }

        public async Task<decimal> GetExchangeFeeCurrencyAsync(string fromCurrency, string toCurrency, decimal price)
        {
            if (fromCurrency.Equals(toCurrency)) return 0;
                var priceFee = await _context.CurrencyExchangeFees.SingleOrDefaultAsync(x =>
                x.FromCurrency.Equals(fromCurrency) && x.ToCurrency.Equals(toCurrency) && x.StartRange < price && x.EndRange > price);
            return priceFee?.PriceFee*price/100 ?? 0;
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
            var currency = await _context.Currencies.AnyAsync(x => x.CurrencyCode.Equals(codeCurrency));
            return currency;

        }

        public async Task<bool> IsExistCurrencyByCodeAsync(string firstCodeCurrency, string secondCodeCurrency)
        {
            var currency = await _context.Currencies.AnyAsync(x => x.CurrencyCode.Equals(firstCodeCurrency) && x.CurrencyCode.Equals(secondCodeCurrency));
            return currency;
        }


        public async Task<decimal> CurrencyConvertor(string fromCurrency, string toCurrency, decimal amount)
        {
            var rate = await GetPriceRateExchange(fromCurrency, toCurrency);
            return amount * rate;
        }

        public async Task<decimal> GetPriceRateExchange(string fromCurrency, string toCurrency)
        {
            if (fromCurrency.Equals(toCurrency)) return 1;

            var rate = await _context.ExchangeRates
            .FirstOrDefaultAsync(x => x.FromCurrency.Equals(fromCurrency) && x.ToCurrency.Equals(toCurrency));
            if (rate==null)
            {
               return await _apiServices.GetExchangeRateAsync(fromCurrency, toCurrency);
            }
            return rate.Rate;
        }

        public async Task<ExchangeRate> GetRateExchange(string fromCurrency, string toCurrency)
        {
            var rate = await _context.ExchangeRates
                .FirstOrDefaultAsync(x => x.FromCurrency.Equals(fromCurrency) && x.ToCurrency.Equals(toCurrency));
            return rate != null ? rate : null;
        }

        public async Task<int> CreateCurrency(CurrencyDto currencyVM)
        {
            var isExistCurrency = await IsExistCurrencyByCodeAsync(currencyVM.CurrencyCode);
            if (isExistCurrency) return 0;
            currencyVM.CurrencyCode=currencyVM.CurrencyCode.ToUpper();
            var currency = _mapper.Map<Currency>(currencyVM);
            await _context.Currencies.AddAsync(currency);
            var queryResult = await _context.SaveChangesAsync();
            return queryResult > 0 ? currency.CurrencyId : 0;
        }


        public async Task<bool> DeleteExchangeFeeToCurrency(int feeId, int currencyId)
        {
            var feesList = await GetCurrencyExchangeFeeAsync(currencyId);
            if (!feesList.Last().FeeId.Equals(feeId)) return false;
            var fee = await _context.CurrencyExchangeFees.SingleOrDefaultAsync(x => x.FeeId.Equals(feeId));
            if (fee != null) _context.CurrencyExchangeFees.Remove(fee);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteTransformFeeToCurrency(int feeId, int currencyId)
        {
            var feesList = await GetCurrencyTransformFeeAsync(currencyId);
            if (!feesList.Last().FeeId.Equals(feeId)) return false;
            var fee = await _context.CurrencyTransformFees.SingleOrDefaultAsync(x => x.FeeId.Equals(feeId));
            if (fee != null) _context.CurrencyTransformFees.Remove(fee);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> CreateExchangeRateCurrency(RateDtos rateVM)
        {
            if (rateVM.Rate is < 0 or > 40)
            {
                return 0;
            }
            if (rateVM.FromCurrency.Equals(rateVM.ToCurrency))
            {
                return 1;
            }

            var existRate = await _context.ExchangeRates
                .FirstOrDefaultAsync(x =>
                    x.FromCurrency.Equals(rateVM.FromCurrency) && x.ToCurrency.Equals(rateVM.ToCurrency));
            if (existRate != null) return 0;
            var rate = _mapper.Map<ExchangeRate>(rateVM);
            await _context.ExchangeRates.AddAsync(rate);
            var queryResult = await _context.SaveChangesAsync();
            return queryResult > 0 ? rate.ExchangeRateId : 0;
        }

        public async Task<bool> UpdateExchangeRateToCurrency(UpdateRateDtos rateVM)
        {
            if (rateVM.Rate is < 0 or > 40)
            {
                return false;
            }
            var existRate = await _context.ExchangeRates.SingleOrDefaultAsync(x => x.ExchangeRateId.Equals(rateVM.ExchangeRateId));
            if (existRate == null) return false;
            existRate.Rate = rateVM.Rate;
            _context.ExchangeRates.Update(existRate);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteExchangeRateCurrency(int rateId)
        {
            var existRate = await _context.ExchangeRates.SingleOrDefaultAsync(x => x.ExchangeRateId.Equals(rateId));
            if (existRate == null) return false;
            _context.ExchangeRates.Remove(existRate);
            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<int> CreateTransformFeeToCurrency(CreateFeeDtos Model)
        {
            if (Model.PriceFee is < 0 or > 40)
            {
                return 0;
            }
            if (Model.FromCurrency.Equals(Model.ToCurrency))
            {
                return 0;
            }
            var lastOldFee = GetListTransformFeesAsync(Model.FromCurrency, Model.ToCurrency).Result.LastOrDefault();
            var fee = _mapper.Map<CurrencyTransformFees>(Model);
            if (lastOldFee != null)
            {
                if (lastOldFee.EndRange > fee.EndRange)
                {
                    return 0;
                }
                fee.StartRange = lastOldFee.EndRange + 1;
            }
            else
            {
                fee.StartRange = 0;
            }
            await _context.CurrencyTransformFees.AddAsync(fee);
            var queryResult = await _context.SaveChangesAsync();
            return queryResult > 0 ? fee.FeeId : 0;
        }

        public async Task<bool> UpdateTransformFeeToCurrency(int feeId, decimal feePrice)
        {
            if (feePrice is < 0 or > 40)
            {
                return false;
            }
            var fee = await _context.CurrencyTransformFees.SingleOrDefaultAsync(x => x.FeeId.Equals(feeId));
            if (fee == null) return false;
            fee.PriceFee = feePrice;
            _context.CurrencyTransformFees.Update(fee);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> CreateExchangeFeeToCurrency(CreateFeeDtos Model)
        {
            if (Model.PriceFee is < 0 or > 40)
            {
                return 0;
            }
            if (Model.ToCurrency.Equals(Model.FromCurrency))
            {
                return 1;
            }
            var lastOldFee =GetListExchangeFeesAsync(Model.FromCurrency, Model.ToCurrency).Result.LastOrDefault();
            var fee = _mapper.Map<CurrencyExchangeFees>(Model);
            if (lastOldFee != null)
            {
                if (lastOldFee.EndRange > fee.EndRange)
                {
                    return 0;
                }
                fee.StartRange = lastOldFee.EndRange + 1;
            }
            else
            {
                fee.StartRange = 0;
            }
            await _context.CurrencyExchangeFees.AddAsync(fee);
            var queryResult = await _context.SaveChangesAsync();
            return queryResult > 0 ? fee.FeeId : 0;
        }

        public async Task<bool> UpdateExchangeFeeToCurrency(int feeId, decimal feePrice)
        {
            if (feePrice is < 0 or > 40) return false;
            var fee = await _context.CurrencyExchangeFees.SingleOrDefaultAsync(x => x.FeeId.Equals(feeId));
            if (fee == null) return false;
            fee.PriceFee = feePrice;
            _context.CurrencyExchangeFees.Update(fee);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
