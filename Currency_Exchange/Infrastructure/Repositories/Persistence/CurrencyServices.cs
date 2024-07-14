﻿using Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Application.Dtos.CurrencyDtos;
using Domain.Entities;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Repositories.Persistence
{
    public class CurrencyServices : ICurrencyServices
    {
        private readonly CurrencyDbContext _context;
        private readonly IMapper _mapper;

        public CurrencyServices(CurrencyDbContext currencyDbContext, IMapper mapper)
        {
            _context = currencyDbContext;
            _mapper = mapper;
        }

        public async Task<Currency?> GetCurrencyByIdAsync(int currencyId)
        {
            return await _context.Currencies.SingleOrDefaultAsync(x => x.CurrencyId.Equals(currencyId));
        }

        public async Task<Currency?> GetCurrencyByCodeAsync(string currencyCode)
        {
            return await _context.Currencies.SingleOrDefaultAsync(x => x.CurrencyCode.Equals(currencyCode));
        }

        public async Task<CurrencyExchangeFees?> GetExchangeFeeCurrencyByIdAsync(int id)
        {
            return await _context.CurrencyExchangeFees.SingleOrDefaultAsync(x => x.FeeId.Equals(id));
        }

        public async Task<List<CurrencyExchangeFees>> GetListExchangeFeesAsync(string fromCurrency, string toCurrency)
        {
            return await _context.CurrencyExchangeFees.Where(x =>
                x.FromCurrency.Equals(fromCurrency) && x.ToCurrency.Equals(toCurrency)).ToListAsync();
        }

        public async Task<decimal> GetTransformFeeCurrencyAsync(string fromCurrency, string toCurrency, decimal price)
        {
            var priceFee=await _context.CurrencyTransformFees.SingleOrDefaultAsync(x =>
                x.FromCurrency.Equals(fromCurrency) && x.ToCurrency.Equals(toCurrency) && x.PriceFee.Equals(price));
            return priceFee?.PriceFee ?? 0;
        }

        public async Task<CurrencyTransformFees?> GetTransformFeeCurrencyByIdAsync(int id)
        {
            return await _context.CurrencyTransformFees.SingleOrDefaultAsync(x => x.FeeId.Equals(id));
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
            return rate != 0 ? amount * rate : 0;

        }

        public async Task<decimal> GetPriceRateExchange(string fromCurrency, string toCurrency)
        {
            if (fromCurrency.Equals(toCurrency)) return 0;

            var rate = await _context.ExchangeRates
            .FirstOrDefaultAsync(x => x.FromCurrency.Equals(fromCurrency) && x.ToCurrency.Equals(toCurrency));
            return rate != null ? rate.Rate : 0;
        }

        public async Task<ExchangeRate> GetRateExchange(string fromCurrency, string toCurrency)
        {
            var rate = await _context.ExchangeRates
                .FirstOrDefaultAsync(x => x.FromCurrency.Equals(fromCurrency) && x.ToCurrency.Equals(toCurrency));
            return rate != null ? rate : null;
        }

        public async Task<int> CreateCurrency(CreateCurrencyDto currencyVM)
        {
            var isExistCurrency = await IsExistCurrencyByCodeAsync(currencyVM.CurrencyCode);
            if (isExistCurrency == false) return 0;
            var Currency = _mapper.Map<Currency>(currencyVM);
            await _context.Currencies.AddAsync(Currency);
            await _context.SaveChangesAsync();
            return Currency.CurrencyId;
        }


        public async Task<int> CreateExchangeRateCurrency(RateDtos rateVM)
        {
            var existRate = await GetPriceRateExchange(rateVM.FromCurrency, rateVM.ToCurrency);
            if (existRate != 0) return 0;
            var rate = _mapper.Map<ExchangeRate>(rateVM);
            await _context.ExchangeRates.AddAsync(rate);
            await _context.SaveChangesAsync();
            return rate.ExchangeRateId;
        }

        public async Task<bool> UpdateExchangeRateToCurrency(RateDtos rateVM)
        {
            var existRate = await GetRateExchange(rateVM.FromCurrency, rateVM.ToCurrency);
            if (existRate == null) return false;
            existRate.Rate = rateVM.Rate;
            _context.ExchangeRates.Update(existRate);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<int> CreateTransformFeeToCurrency(CreateFeeDtos Model)
        {
            var lastOldFee = GetListTransformFeesAsync(Model.FromCurrency, Model.ToCurrency).Result.LastOrDefault();
            var fee = _mapper.Map<CurrencyTransformFees>(Model);
            if (lastOldFee != null)
            {
                if (lastOldFee.EndRange < fee.EndRange)
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
            await _context.SaveChangesAsync();
            return fee.FeeId;
        }

        public async Task<bool> UpdateTransformFeeToCurrency(UpdateFeeDtos Model)
        {
            var fee = await GetExchangeFeeCurrencyByIdAsync(Model.FeeId);
            if (fee == null) return false;
            fee.PriceFee = Model.PriceFee;
            return true;
        }

        public async Task<int> CreateExchangeFeeToCurrency(CreateFeeDtos Model)
        {
            var lastOldFee = GetListExchangeFeesAsync(Model.FromCurrency, Model.ToCurrency).Result.LastOrDefault();
            var fee = _mapper.Map<CurrencyExchangeFees>(Model);
            if (lastOldFee != null)
            {
                if (lastOldFee.EndRange < fee.EndRange)
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
            await _context.SaveChangesAsync();
            return fee.FeeId;
        }

        public async Task<bool> UpdateExchangeFeeToCurrency(UpdateFeeDtos Model)
        {
            var fee = await GetExchangeFeeCurrencyByIdAsync(Model.FeeId);
            if (fee == null) return false;
            fee.PriceFee = Model.PriceFee;
            return true;

        }
    }
}
