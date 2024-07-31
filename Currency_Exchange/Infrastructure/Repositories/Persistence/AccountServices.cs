using System.Drawing.Printing;
using System.Linq;
using Application.Contracts.Persistence;
using Application.Dtos.AccountDtos;
using Application.Dtos.OthersAccountDto;
using Application.Dtos.TransactionDtos;
using Application.Statics;
using AutoMapper;
using Domain.Entities;
using Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding;

namespace Infrastructure.Repositories.Persistence
{
    public class AccountServices : IAccountServices
    {
        private readonly CurrencyDbContext _context;
        private readonly ICurrencyServices _currency;
        private readonly IOthersAccountServices _othersAccountServices;
        
        private readonly IMapper _mapper;

        public AccountServices(CurrencyDbContext context, ICurrencyServices currency, IMapper mapper, IOthersAccountServices othersAccountServices)
        {
            _context = context;
            _currency = currency;
            _mapper = mapper;
            _othersAccountServices = othersAccountServices;
        }
        public async Task<AccountViewModel> GetAccountByIdAsync(string userId, int accountId)
        {
            var account = await _context.Accounts.SingleOrDefaultAsync(x => x.AccountId.Equals(accountId) && x.UserId.Equals(userId));
            return _mapper.Map<AccountViewModel>(account);
        }

        public async Task<AccountViewModel> GetAccountByIdAsync(int accountId)
        {
            var account = await _context.Accounts.SingleOrDefaultAsync(x => x.AccountId.Equals(accountId));
            return _mapper.Map<AccountViewModel>(account);
        }

        public async Task<List<OtherAccountViewModel>> GetAccountsListAsync(string username)
        {
            var favoriteAccounts = new List<OtherAccountViewModel>();
            var userAccounts = await GetListAccountsByNameAsync(username);
            foreach (var item in userAccounts)
            {
                var favoriteAccountDto = new OtherAccountViewModel();
                favoriteAccountDto = _mapper.Map<OtherAccountViewModel>(item);
                favoriteAccountDto.RealAccountId = item.AccountId;
                favoriteAccounts.Add(favoriteAccountDto);
            }
            favoriteAccounts.AddRange(await _othersAccountServices.GetListOthersAccountsByNameAsync(username));
            return favoriteAccounts;
        }

        public async Task<UpdateAccountViewModel> GetAccountByIdForUpdateAsync(string userId, int accountId)
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
            var accounts = await _context.Accounts.IgnoreQueryFilters().Where(x => x.UserId.Equals(userId) && x.IsDeleted).ToListAsync();
            return _mapper.Map<List<AccountViewModel>>(accounts);
        }

        public async Task<bool> IsAccountExist(int accountId)
        {
            return await _context.Accounts.AnyAsync(x => x.AccountId.Equals(accountId));
        }

        public async Task<int> CreateAccountAsync(CreateAccountViewModel accountVM)
        {
            // Validate
            if (!await CheckMinimumBalanceCondition(accountVM.Currency,accountVM.Balance)) return 0;
            var cartNumber = CartNumbers.GenerateUnique16DigitNumbers();
            while (await IsCartNumberExist(cartNumber) == false)
            {
                cartNumber = CartNumbers.GenerateUnique16DigitNumbers();
            }
            // processes
            var account = _mapper.Map<Account>(accountVM);
            account.CartNumber = cartNumber;
            await _context.Accounts.AddAsync(account);
            var queryResult= await _context.SaveChangesAsync();
            return queryResult>0 ? account.AccountId : 0;
        }

        private async Task<bool> CheckMinimumBalanceCondition(string currency,decimal balance)
        {
            if (!await _currency.IsExistCurrencyByCodeAsync(currency)) return false;
            var amount = await _currency.ConvertCurrencyAsync(currency, "USD", balance);
            return amount > BusinessConstants.MinBalance;
        }

        public async Task<int> UpdateAccountAsync(UpdateAccountViewModel accountVM, string userid)
        {
            //validate
            var account = await _context.Accounts.SingleOrDefaultAsync(x => x.AccountId.Equals(accountVM.AccountId) && x.UserId.Equals(userid) && x.Currency.Equals(accountVM.Currency));
            if (account == null) return 0;
            if (!await CheckMinimumBalanceCondition(accountVM.Currency, accountVM.Balance)) return 0;
            // processes
            account.Balance = accountVM.Balance;
            account.AccountName = accountVM.AccountName;
            _context.Accounts.Update(account);
            var queryResult = await _context.SaveChangesAsync();
            return queryResult > 0 ? account.AccountId : 0;
        }

        public async Task<int> UpdateAccountBalanceAsync(UpdateAccountViewModel accountVM)
        {
            if (!await _currency.IsExistCurrencyByCodeAsync(accountVM.Currency)) return 0;
            var account = await _context.Accounts.SingleOrDefaultAsync(x => x.AccountId.Equals(accountVM.AccountId) && x.Currency.Equals(accountVM.Currency));
            if (account == null) return 0;
            // processes
            account.Balance = accountVM.Balance;
            _context.Accounts.Update(account);
            var queryResult = await _context.SaveChangesAsync();
            return queryResult > 0 ? account.AccountId : 0;
        }

        public async Task<bool> DeleteAccountAsync(int accountId, string userId)
        {
            //validate
            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.AccountId.Equals(accountId) && x.UserId.Equals(userId));
            if (account == null) return false;
            // processes
            account.Balance = 0;
            account.IsDeleted = true;

            _context.Accounts.Update(account);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> IncreaseAccountBalanceAsync(IncreaseBalanceDto balanceDto, string username)
        {
            //validate
            if (balanceDto.Amount < 0) return false;
            if (!await _currency.IsExistCurrencyByCodeAsync(balanceDto.FromCurrency)) return false;
            if (!await _currency.IsExistCurrencyByCodeAsync(balanceDto.ToCurrency)) return false;
            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.AccountId.Equals(balanceDto.AccountId) && x.UserId.Equals(username));
            if (account == null) return false;
            if (!balanceDto.ToCurrency.Equals(account.Currency)) return false;
            // processes
            if (balanceDto.FromCurrency != balanceDto.ToCurrency)
            {
                account.Balance += await _currency.ConvertCurrencyAsync(balanceDto.FromCurrency, balanceDto.ToCurrency, balanceDto.Amount);
            }
            else
            {
                account.Balance += balanceDto.Amount;
            }
            var transaction = new Transaction()
            {
                FromCurrency = balanceDto.FromCurrency,
                ToCurrency = account.Currency,
                UserBalance = account.Balance,
                Amount = balanceDto.Amount,
                CompletedAt = DateTime.UtcNow,
                FromAccountId = account.AccountId,
                ToAccountId = account.AccountId,
                ExchangeRate =await _currency.GetPriceRateExchangeAsync(balanceDto.FromCurrency,balanceDto.ToCurrency),
                Status = StatusEnum.Completed,
                Outer = false,
                UserId = username
            };
            await _context.Transactions.AddAsync(transaction);
            _context.Accounts.Update(account);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsCartNumberExist(string cartNumber)
        {
            var result = await _context.Accounts.SingleOrDefaultAsync(x => x.CartNumber.Equals(cartNumber));
            if (result == null) return true;
            return false;
        }

        public async Task<List<TransactionDto>> GetAccountTransactionsAsync(int accountId)
        {
            var transactions = await _context.Transactions.Where(x => x.FromAccountId == accountId).ToListAsync();
            return _mapper.Map<List<TransactionDto>>(transactions);
        }

        public async Task<List<UsersTransactionsDto>> GetUserAccountTransactionsAsync(int accountId)
        {
            var transactions = await _context.Transactions.Where(x => x.FromAccountId == accountId||x.ToAccountId.Equals(accountId)).ToListAsync();
            var transactionDto = new List<UsersTransactionsDto>();
            foreach (var item in transactions)
            {
                var dto = _mapper.Map<UsersTransactionsDto>(item);
                if (accountId.Equals(item.ToAccountId)&&!item.ToAccountId.Equals(item.FromAccountId))
                {
                    dto.FromSender = false;
                }

                var account = _context.Accounts.IgnoreQueryFilters().SingleOrDefaultAsync(x => x.AccountId.Equals(item.ToAccountId)).Result;
                if (account != null)
                    dto.ToAccountId = account.AccountName;
                transactionDto.Add(dto);
            }
            return transactionDto;
        }

        public async Task<List<UsersTransactionsDto>> GetUserTransactionsAsync(string userId)
        {
            // var transaction = await _context.Users.Include(x => x.Transactions)
            //     .Where(x => x.Id.Equals(userId))
            //     .SelectMany(x => x.Transactions).ToListAsync();
            var transactions = await _context.Accounts.Include(x => x.Transactions)
                .Where(x => x.UserId.Equals(userId)).SelectMany(x => x.Transactions).ToListAsync();
            return _mapper.Map<List<UsersTransactionsDto>>(transactions);
        }


        public async Task<bool> WithdrawalAsync(int accountId, string userId, decimal amount)
        {
            //Validate
            if (amount < 0) return false;
            var account = await _context.Accounts.SingleOrDefaultAsync(x => x.AccountId.Equals(accountId) && x.UserId.Equals(userId));
            if (account == null) return false;
            var dollar = await _currency.ConvertCurrencyAsync(account.Currency, "USD", account.Balance - amount);
            if (dollar < BusinessConstants.MinBalance) return false;
            // processes
            account.Balance -= amount;
            var transaction = new Transaction()
            {
                FromCurrency = account.Currency,
                ToCurrency = account.Currency,
                Amount = amount,
                DeductedAmount = amount,
                UserBalance = account.Balance,
                CompletedAt = DateTime.UtcNow,
                FromAccountId = accountId,
                ToAccountId = accountId,
                Status = StatusEnum.Completed,
                UserId = userId,
                Outer = true
            };
            await _context.Transactions.AddAsync(transaction);
            _context.Accounts.Update(account);
            return await _context.SaveChangesAsync() > 0;
             
        }

        public async Task<bool> AccountAddressAsync(int accountId, string userId,string address="")
        {
            //validate
            var account = await _context.Accounts.SingleOrDefaultAsync(x => x.AccountId.Equals(accountId) && x.UserId.Equals(userId));
            if (account == null) return false;
            var accountDeleteInfo =await _context.DeletedAccounts.IgnoreQueryFilters().FirstOrDefaultAsync(x =>
                    x.AccountId.Equals(accountId) && x.UserId.Equals(userId));
            if (accountDeleteInfo != null)
            {
                // Delete old version
               _context.DeletedAccounts.Remove(accountDeleteInfo);
               await _context.SaveChangesAsync();
            }
            // processes
            var addressInfo = new DeletedAccount()
            {
                UserId = userId,
                Accepted = false,
                AccountId = accountId,
                CompleteTime = DateTime.UtcNow,
                Address = address,
                Balance = account.Balance,

            };
            await _context.DeletedAccounts.AddAsync(addressInfo);
            return await _context.SaveChangesAsync() > 0;
          
        }

        public async Task<ConfirmAddressAccountForDeleteDto> GetConfirmAccountDeleteInfoAsync(int accountId, string userId)
        {
            return _mapper.Map<ConfirmAddressAccountForDeleteDto>( await _context.DeletedAccounts.IgnoreQueryFilters().OrderBy(x=>x.CompleteTime)
                    .LastOrDefaultAsync(x => x.UserId.Equals(userId) && x.AccountId.Equals(accountId)));
        }

        public async Task<bool> ConfirmAccountDeleteInfoAsync(int accountId, string userId)
        {
            // validate
            var deleteAccount =await _context.DeletedAccounts.IgnoreQueryFilters().SingleOrDefaultAsync(x =>
                    x.UserId.Equals(userId) && x.AccountId.Equals(accountId));
            if (deleteAccount == null) return false;
            // processes
            deleteAccount.Accepted = true;
            _context.DeletedAccounts.Update(deleteAccount);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<ConfirmAddressAccountForDeleteDto>> GetAccountDeleteInfo(string userId)
        {
            return _mapper.Map<List<ConfirmAddressAccountForDeleteDto>>(await _context.DeletedAccounts
                .Where(x => x.UserId.Equals(userId)).ToListAsync());
        }

       
        public async Task<bool> IsAccountForUserAsync(string username, int accountId)
        {
            return await _context.Accounts.AnyAsync(x => x.UserId.Equals(username) && x.AccountId.Equals(accountId));
        }


    }
}
