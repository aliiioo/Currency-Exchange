﻿using Application.Dtos.AccountDtos;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Application.Dtos.OthersAccountDto;
using Application.Dtos.CurrencyDtos;
using Application.Dtos.TransactionDtos;

namespace Infrastructure.Mappings
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Account, AccountViewModel>().ReverseMap();
            CreateMap<Account, UpdateAccountViewModel>().ReverseMap();
            CreateMap<Account, OtherAccountViewModel>().ReverseMap();
            CreateMap<Currency, CurrencyDto>().ReverseMap();
            CreateMap<CurrencyExchangeFees, CreateFeeDtos>().ReverseMap();
            CreateMap<CurrencyExchangeFees, UpdateFeeDtos>().ReverseMap();
            CreateMap<CurrencyTransformFees, UpdateFeeDtos>().ReverseMap();
            CreateMap<CurrencyTransformFees, CreateFeeDtos>().ReverseMap();
            CreateMap<ExchangeRate, RateDtos>().ReverseMap();
            CreateMap<ExchangeRate, UpdateRateDtos>().ReverseMap();
            CreateMap<Transaction, TransactionDto>().ReverseMap();
            CreateMap<Transaction, CreateTransactionDtos>().ReverseMap();
            CreateMap<Currency, CurrencyDetailDto>().ReverseMap();
        }
    }
}
