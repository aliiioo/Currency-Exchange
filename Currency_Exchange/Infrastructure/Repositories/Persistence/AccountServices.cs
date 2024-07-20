using Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Update;

namespace Infrastructure.Repositories.Persistence
{
    public class AccountServices : IAccountServices
    {
        private readonly CurrencyDbContext _context;
        private readonly ICurrencyServices _currency;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountServices(CurrencyDbContext context, ICurrencyServices currency, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _currency = currency;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<AccountViewModel> GetAccountByIdAsync(string userId, int accountId)
        {
            var account = await _context.Accounts.SingleOrDefaultAsync(x => x.AccountId.Equals(accountId) && x.UserId.Equals(userId));
            return _mapper.Map<AccountViewModel>(account);
        }

        public async Task<UpdateAccountViewModel> GetAccountByIdAsyncForUpdate(string userId, int accountId)
        {
            var account = await _context.Accounts.SingleOrDefaultAsync(x => x.AccountId.Equals(accountId) && x.UserId.Equals(userId));
            return _mapper.Map<UpdateAccountViewModel>(account);
        }

        public async Task<List<AccountViewModel>> GetListAccountsByNameAsync(string userId)
        {
            var accounts = await _context.Accounts.Where(x => x.UserId.Equals(userId)).ToListAsync();
            return _mapper.Map<List<AccountViewModel>>(accounts);
        }

        public async Task<List<AccountViewModel>> GetListDeleteAccountsByNameAsync(string userId)
        {
            var accounts = await _context.Accounts.IgnoreQueryFilters().Where(x => x.UserId.Equals(userId)&&x.IsDeleted).ToListAsync();
            return _mapper.Map<List<AccountViewModel>>(accounts);
        }

        public async Task<int> CreateAccount(CreateAccountViewModel accountVM)
        {
            var existCurrency = await _currency.IsExistCurrencyByCodeAsync(accountVM.Currency);
            if (!existCurrency) return 0;
            var amount = await _currency.CurrencyConvertor(accountVM.Currency, "USD", accountVM.Balance);
            if (amount < MinimumAmount.MinBalance) return 0;

            var account = _mapper.Map<Account>(accountVM);
            var cartNumber = CartNumbers.GenerateUnique16DigitNumbers();
            while (await IsCartNumberExist(cartNumber) == false)
            {
                cartNumber=CartNumbers.GenerateUnique16DigitNumbers();
            }
            account.CartNumber=cartNumber;
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
            return account.AccountId;

        }

        public async Task<int> UpdateAccount(UpdateAccountViewModel accountVM)
        {
            var amount = await _currency.CurrencyConvertor(accountVM.Currency, "USD", accountVM.Balance);
            if (amount < MinimumAmount.MinBalance) return 0;
            var account = await _context.Accounts.SingleOrDefaultAsync(x => x.AccountId.Equals(accountVM.AccountId) && x.UserId.Equals(accountVM.UserId));
            if (account == null) return 0;

            account.Balance -= accountVM.Balance;
            account.AccountName = accountVM.AccountName;

            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            return account.AccountId;



        }

        public async Task<bool> DeleteAccountAsync(int accountId, string userId)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.AccountId.Equals(accountId) && x.UserId.Equals(userId));
            if (account == null) return false;
            account.IsDeleted=true;
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IncreaseAccountBalance(IncreaseBalanceDto balanceDto, string username)
        {
            var amount = new decimal();
            if (balanceDto.FromCurrency != balanceDto.ToCurrency)
            {
                amount = await _currency.CurrencyConvertor(balanceDto.FromCurrency, balanceDto.ToCurrency, balanceDto.Amount);
            }
            else
            {
                amount = balanceDto.Amount;
            }
            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.AccountId.Equals(balanceDto.AccountId) && x.UserId.Equals(username));
            if (account == null) return false;
            account.Balance += amount;
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> IsCartNumberExist(string cartNumber)
        {
            var result = await _context.Accounts.SingleOrDefaultAsync(x => x.CartNumber.Equals(cartNumber));
            if (result == null) return true;
            return false;
        }

        public async Task<bool> IsAccountForUser(string username, int accountId)
        {
            return await _context.Accounts.AnyAsync(x => x.UserId.Equals(username) && x.AccountId.Equals(accountId));
        }


    }
}
