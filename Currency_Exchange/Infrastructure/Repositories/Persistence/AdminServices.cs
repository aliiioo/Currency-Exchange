﻿using Application.Contracts.Persistence;
using Application.Dtos.AccountDtos;
using AutoMapper;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Persistence
{



    public class AdminServices : IAdminServices
    {
        private readonly CurrencyDbContext _context;
        private readonly IMapper _mapper;
        

        public AdminServices(CurrencyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<List<AccountViewModel>> GetAccountsForAdminAsync()
        {
            var accounts = await _context.Accounts.ToListAsync();
            return _mapper.Map<List<AccountViewModel>>(accounts);
        }

        public async Task<List<AccountViewModel>> GetDeletedAccountsForAdminAsync()
        {
            var accounts = await _context.Accounts.IgnoreQueryFilters().Where(x => x.IsDeleted).ToListAsync();
            return _mapper.Map<List<AccountViewModel>>(accounts);
        }

        public async Task<List<AccountViewModel>> GetDisActiveAccountsForAdminAsync()
        {
            var accounts = await _context.Accounts.IgnoreQueryFilters().Where(x => !x.IsDeleted && !x.IsActive).ToListAsync();
            return _mapper.Map<List<AccountViewModel>>(accounts);
        }

        public async Task<AccountViewModel> GetAccountByIdForAdminAsync(int accountId)
        {
            return _mapper.Map<AccountViewModel>(await _context.Accounts.SingleOrDefaultAsync(x => x.AccountId.Equals(accountId)));
        }

        public async Task<AccountViewModel> GetAccountByCartNumberForAdminAsync(string cartNumber)
        {
            return _mapper.Map<AccountViewModel>(await _context.Accounts.SingleOrDefaultAsync(x => x.CartNumber.Equals(cartNumber)));
        }

        public async Task<List<AccountViewModel>> GetUsersAccountsForAdminAsync(string email)
        {
            var account = await _context.Users.Where(x => x.Email.Equals(email)).SelectMany(x => x.Accounts)
                .ToListAsync();
            return _mapper.Map<List<AccountViewModel>>(account);

        }

        public async Task<List<DeleteAccountAddressDto>> GetAccountDeleteInfoForAdminAsync()
        {
            return _mapper.Map<List<DeleteAccountAddressDto>>(await _context.DeletedAccounts.ToListAsync());

        }

        public async Task<bool> ActivateAccountAsync(int accountId)
        {
            var account = await _context.Accounts.IgnoreQueryFilters().SingleOrDefaultAsync(x => x.AccountId.Equals(accountId));
            if (account == null) return false;
            account.IsActive = true;
            _context.Accounts.Update(account);
            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<bool> DeActivateAccountAsync(int accountId)
        {
            var account = await _context.Accounts.SingleOrDefaultAsync(x => x.AccountId.Equals(accountId));
            if (account == null) return false;
            account.IsActive = false;
            _context.Accounts.Update(account);
            return await _context.SaveChangesAsync() > 0;
           

        }

    }
}
