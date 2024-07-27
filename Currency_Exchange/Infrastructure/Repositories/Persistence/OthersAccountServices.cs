using Application.Contracts.Persistence;
using Application.Dtos.OthersAccountDto;
using Application.Statics;
using AutoMapper;
using Domain.Entities;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Persistence
{
    public class OthersAccountServices : IOthersAccountServices
    {
        private readonly CurrencyDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrencyServices _currency;
        private readonly IAccountServices _accountServices;

        public OthersAccountServices(CurrencyDbContext context, IMapper mapper, ICurrencyServices currency, IAccountServices accountServices)
        {
            _context = context;
            _mapper = mapper;
            _currency = currency;
            _accountServices = accountServices;
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
            var accounts = await _context.OthersAccounts.SingleOrDefaultAsync(x => x.AccountId.Equals(accountId) && x.UserId.Equals(username));
            return _mapper.Map<OtherAccountViewModel>(accounts);
        }


        public async Task<int> CreateOthersAccountAsync(CreateOtherAccountViewModel accountVM)
        {
            //validate
            if (!ValidateCartNumber.IsValidCardNumber(accountVM.CartNumber)) return 0;
            var existCurrency = await _currency.IsExistCurrencyByCodeAsync(accountVM.Currency);
            if (!existCurrency) return 0;
            var account =await _context.Accounts.SingleOrDefaultAsync(x => x.CartNumber.Equals(accountVM.CartNumber) && x.Currency.Equals(accountVM.Currency));
            if (account == null) return 0;
            // processes
            var newOtherAccount = _mapper.Map<OthersAccount>(accountVM);
            newOtherAccount.RealAccountId = account.AccountId;
            await _context.OthersAccounts.AddAsync(newOtherAccount);
            var queryResult = await _context.SaveChangesAsync();
            return queryResult > 0 ? newOtherAccount.AccountId : 0;
        }

        public async Task<bool> DeleteOthersAccountAsync(int accountId, string userId)
        {
            // validate
            var account = await _context.OthersAccounts.SingleOrDefaultAsync(x => x.AccountId.Equals(accountId) && x.UserId.Equals(userId));
            if (account == null) return false;
            // processes
            _context.OthersAccounts.Remove(account);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> UpdateOthersAccountAsync(UpdateOtherAccountViewModel otherAccountViewModel, string userId)
        {
            //validate
            if (!ValidateCartNumber.IsValidCardNumber(otherAccountViewModel.CartNumber)) return 0;
            var definedAccount = await _context.OthersAccounts.SingleOrDefaultAsync(x => x.AccountId.Equals(otherAccountViewModel.AccountId) && x.UserId.Equals(userId));
            if (definedAccount == null) return 0;
            var account=await _context.Accounts.SingleOrDefaultAsync(x=>x.CartNumber.Equals(otherAccountViewModel.CartNumber)&&x.Currency.Equals(definedAccount.Currency));
            if (account == null) return 0;
            // processes
            definedAccount.AccountName = otherAccountViewModel.AccountName;
            definedAccount.CartNumber = otherAccountViewModel.CartNumber;
            definedAccount.RealAccountId=account.AccountId;
            _context.OthersAccounts.Update(definedAccount);
            var queryResult = await _context.SaveChangesAsync();
            return queryResult > 0 ? definedAccount.AccountId : 0;
        }

        public async Task<bool> IsAccountForOthers(string username, int accountId)
        {
            return await _context.OthersAccounts.AnyAsync(x => x.UserId.Equals(username) && x.AccountId.Equals(accountId));
        }

    }
}
