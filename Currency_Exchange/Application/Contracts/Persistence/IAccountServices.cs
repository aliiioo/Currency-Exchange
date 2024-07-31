using Application.Dtos.AccountDtos;
using Application.Dtos.ErrorsDtos;
using Application.Dtos.OthersAccountDto;
using Application.Dtos.TransactionDtos;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Application.Contracts.Persistence
{
    public interface IAccountServices
    {
        public Task<AccountViewModel> GetAccountByIdAsync(string userId, int accountId);
        public Task<AccountViewModel> GetAccountByIdAsync(int accountId);
        public Task<List<OtherAccountViewModel>> GetAccountsListAsync(string username);
        public Task<UpdateAccountViewModel> GetAccountByIdForUpdateAsync(string userId, int accountId);
        public Task<List<AccountViewModel>> GetListAccountsByNameAsync(string userId);
        public Task<List<AccountViewModel>> GetListDeleteAccountsByNameAsync(string userId);
        public Task<bool> IsAccountForUserAsync(string username, int accountId);
        public Task<bool> IsAccountExist(int accountId);
        public Task<int> CreateAccountAsync(CreateAccountViewModel accountVM);
        public Task<int> UpdateAccountAsync(UpdateAccountViewModel accountVM,string userid);
        public Task<int> UpdateAccountBalanceAsync(UpdateAccountViewModel accountVM);
        public Task<bool> DeleteAccountAsync(int accountId, string userId);
        public Task<bool> IncreaseAccountBalanceAsync(IncreaseBalanceDto balanceDto,string username);
        public Task<bool> IsCartNumberExist(string cartNumber);
        public Task<List<TransactionDto>> GetAccountTransactionsAsync(int accountId);
        public Task<List<UsersTransactionsDto>> GetUserAccountTransactionsAsync(int accountId);
        public Task<List<UsersTransactionsDto>> GetUserTransactionsAsync(string userId);
        public Task<bool> WithdrawalAsync(int accountId,string userId,decimal amount);
        public Task<bool> AccountAddressAsync(int accountId, string userId,string address="");
        public Task<ConfirmAddressAccountForDeleteDto> GetConfirmAccountDeleteInfoAsync(int accountId,string userId);
        public Task<bool> ConfirmAccountDeleteInfoAsync(int accountId,string userId);
        public Task<List<ConfirmAddressAccountForDeleteDto>> GetAccountDeleteInfo(string userId);




    }
}
