using Application.Contracts.Persistence;
using Application.Dtos.AccountDtos;
using AutoMapper;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<List<AccountViewModel>> GetDisActiveAccountsForAdmin()
        {
            var accounts = await _context.Accounts.IgnoreQueryFilters().Where(x => !x.IsDeleted&&!x.IsActive).ToListAsync();
            return _mapper.Map<List<AccountViewModel>>(accounts);
        }

        public async Task<bool> ActiveAccount(int accountId)
        {
            var account =await _context.Accounts.SingleOrDefaultAsync(x => x.AccountId.Equals(accountId));
            if (account == null) return false;
            account.IsActive=true;
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> DisActiveAccount(int accountId)
        {
            var account = await _context.Accounts.SingleOrDefaultAsync(x => x.AccountId.Equals(accountId));
            if (account == null) return false;
            account.IsActive=false;
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            return true;

        }
    }
}
