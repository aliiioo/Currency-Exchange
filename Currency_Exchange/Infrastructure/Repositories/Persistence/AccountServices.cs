using Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Application.Dtos.AccountDtos;
using AutoMapper;
using Domain.Entities;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Application.Statics;
using Microsoft.EntityFrameworkCore.Update;

namespace Infrastructure.Repositories.Persistence
{
    public class AccountServices: IAccountServices
    {
        private readonly CurrencyDbContext  _context;
        private readonly ICurrencyServices _currency;
        private readonly IMapper _mapper;

        public AccountServices(CurrencyDbContext context, ICurrencyServices currency, IMapper mapper)
        {
            _context = context;
            _currency = currency;
            _mapper = mapper;
        }
        public async Task<AccountViewModel> GetAccountByIdAsync(string username,int accountId)
        {
            var account=await _context.Accounts.SingleOrDefaultAsync(x => x.AccountId.Equals(accountId)&&x.UserId.Equals(username));
            return _mapper.Map<AccountViewModel>(account);
        }

        public async Task<List<AccountViewModel>> GetListAccountsByNameAsync(string username)
        {
            var accounts = await _context.Accounts.Where(x => x.UserId.Equals(username)).ToListAsync();
            return _mapper.Map<List<AccountViewModel>>(accounts);
        }

        public async Task<int> CreateAccount(CreateAccountViewModel accountVM)
        {
            var existCurrency = await _currency.IsCurrencyByCodeAsync(accountVM.Currency);
            if (!existCurrency) return 0;
            var amount=await _currency.CurrencyConvertor(accountVM.Currency, "USD", accountVM.Balance);
            if (amount < MinimumAmount.MinBalance) return 0;

            var newAccount = _mapper.Map<Account>(accountVM);
            await _context.Accounts.AddAsync(newAccount);
            await _context.SaveChangesAsync();
            return newAccount.AccountId;

        }

        public async Task<int> UpdateAccount(UpdateAccountViewModel accountVM)
        {
            var amount = await _currency.CurrencyConvertor(accountVM.Currency, "USD", accountVM.Balance);
            if (amount < MinimumAmount.MinBalance) return 0;
            var account=await _context.Accounts.SingleOrDefaultAsync(x=>x.AccountId.Equals(accountVM.AccountId)&&x.UserId.Equals(accountVM.UserId));
            if (account == null) return 0;

            account.Balance=accountVM.Balance;
            account.AccountName=accountVM.AccountName;

            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            return account.AccountId;



        }

        public async Task DeleteAccountAsync(int accountId)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(x=>x.AccountId==accountId);
            if (account != null) _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
        }

        public async Task IncreaseAccountBalance(int accountId, int amount)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.AccountId == accountId);
            if (account!=null) account.Balance += amount;
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();

        }
    }
}
