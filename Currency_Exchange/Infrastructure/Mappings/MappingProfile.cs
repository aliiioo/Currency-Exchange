﻿using Application.Dtos.AccountDtos;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Application.Dtos.OthersAccountDto;

namespace Infrastructure.Mappings
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Account, AccountViewModel>().ReverseMap();
            CreateMap<Account, OtherAccountViewModel>().ReverseMap();

        }
    }
}
