using Application.Dtos.AccountDtos;
using Application.Dtos.CurrencyDtos;
using Application.Dtos.OthersAccountDto;
using Application.Dtos.TransactionDtos;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Account, AccountViewModel>().ReverseMap();
            CreateMap<Account, UpdateAccountViewModel>().ReverseMap();
            CreateMap<Account, OtherAccountViewModel>().ReverseMap();
            CreateMap<Account, CreateAccountViewModel>().ReverseMap();
            CreateMap<Currency, CurrencyDto>().ReverseMap();
            CreateMap<Currency, CurrencyDtoShow>().ReverseMap();
            CreateMap<CurrencyExchangeFees, CreateFeeDtos>().ReverseMap();
            CreateMap<CurrencyExchangeFees, UpdateFeeDtos>().ReverseMap();
            CreateMap<CurrencyTransformFees, UpdateFeeDtos>().ReverseMap();
            CreateMap<CurrencyTransformFees, CreateFeeDtos>().ReverseMap();
            CreateMap<ExchangeRate, RateDtos>().ReverseMap();
            CreateMap<ExchangeRate, UpdateRateDtos>().ReverseMap();
            CreateMap<Transaction, TransactionDto>().ReverseMap();
            CreateMap<Transaction, CreateTransactionDtos>()
                .ForMember(x => x.SelfAccountId, z => z.MapFrom(s => s.FromAccountId))
                .ForMember(x => x.UserId, z => z.MapFrom(s => s.UserId))
                .ReverseMap();
            CreateMap<Currency, CurrencyDetailDto>().ReverseMap();
            CreateMap<OthersAccount, CreateOtherAccountViewModel>().ReverseMap();
            CreateMap<OthersAccount, UpdateOtherAccountViewModel>().ReverseMap();
            CreateMap<OthersAccount, UpdateOtherAccountViewModel>().ReverseMap();
            CreateMap<OtherAccountViewModel, UpdateOtherAccountViewModel>().ReverseMap();
            CreateMap<ApplicationUser, CreateAccountViewModel>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id)).ReverseMap();
            CreateMap<Transaction, TransactionDto>().ReverseMap();
            CreateMap<Transaction, ConfirmTransactionDto>().ReverseMap();
            CreateMap<OthersAccount, OtherAccountViewModel>().ReverseMap();
            CreateMap<AccountViewModel, UpdateAccountViewModel>().ReverseMap();
            CreateMap<DeletedAccount, ConfirmAddressAccountForDeleteDto>().ReverseMap();
            CreateMap<DeletedAccount, DeleteAccountAddressDto>().ReverseMap();


        }
    }
}
