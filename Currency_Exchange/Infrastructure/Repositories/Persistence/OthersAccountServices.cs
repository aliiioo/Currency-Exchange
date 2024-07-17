using Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Application.Dtos.OthersAccountDto;
using Application.Dtos.AccountDtos;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Infrastructure.DbContexts;
using Application.Statics;
using Domain.Entities;

namespace Infrastructure.Repositories.Persistence
{
    public class OthersAccountServices: IOthersAccountServices
    {
        private readonly CurrencyDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrencyServices _currency;

        public OthersAccountServices(CurrencyDbContext context, IMapper mapper, ICurrencyServices currency)
        {
            _context = context;
            _mapper = mapper;
            _currency = currency;
        }
        public async Task<List<OtherAccountViewModel>> GetListOthersAccountsByNameAsync(string username)
        {
            var accounts = await _context.OthersAccounts.Where(x => x.UserId.Equals(username)).ToListAsync();
            return _mapper.Map<List<OtherAccountViewModel>>(accounts);
        }

        public async Task<UpdateOtherAccountViewModel> GetOtherAccountByNameForUpdateAsync(int accountId)
        {
            var accounts = await _context.OthersAccounts.SingleOrDefaultAsync(x => x.AccountId.Equals(accountId));
            return _mapper.Map<UpdateOtherAccountViewModel>(accounts);

        }

        public async Task<OtherAccountViewModel> GetOtherAccountByIdAsync(int accountId, string username)
        {
            var accounts = await _context.OthersAccounts.SingleOrDefaultAsync(x => x.AccountId.Equals(accountId)&&x.UserId.Equals(username));
            return _mapper.Map<OtherAccountViewModel>(accounts);
        }


        public async Task<int> CreateOthersAccountAsync(CreateOtherAccountViewModel accountVM)
        {
            var existCurrency = await _currency.IsExistCurrencyByCodeAsync(accountVM.Currency);
            if (!existCurrency) return 0;
            var newOtherAccount = _mapper.Map<OthersAccount>(accountVM);
            await _context.OthersAccounts.AddAsync(newOtherAccount);
            await _context.SaveChangesAsync();
            return newOtherAccount.AccountId;
        }

        public async Task<bool> DeleteOthersAccountAsync(int accountId ,string username)
        {
            var account=await _context.OthersAccounts.SingleOrDefaultAsync(x=>x.AccountId.Equals(accountId)&&x.UserId.Equals(username));
            if (account!=null)
            {
                _context.OthersAccounts.Remove(account);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;

        }

        public async Task<int> UpdateOthersAccountAsync(UpdateOtherAccountViewModel otherAccountViewModel)
        {            
            var account = await _context.OthersAccounts.SingleOrDefaultAsync(x => x.AccountId.Equals(otherAccountViewModel.AccountId) && x.UserId.Equals(otherAccountViewModel.UserId));
            if (account == null) return 0;
            account.AccountName = otherAccountViewModel.AccountName;
            account.Balance=otherAccountViewModel.Balance;
            _context.OthersAccounts.Update(account);
            await _context.SaveChangesAsync();
            return account.AccountId;
        }

        public async Task<bool> IsAccountForOthers(string username, int accountId)
        {
            return await _context.OthersAccounts.AnyAsync(x => x.UserId.Equals(username) && x.AccountId.Equals(accountId));
        }

    }
}
