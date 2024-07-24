using Application.Dtos.AccountDtos;
using Application.Dtos.TransactionDtos;

namespace Application.Contracts.Persistence
{
    public interface IAccountServices
    {
        public Task<AccountViewModel> GetAccountByIdAsync(string userId, int accountId);
      
        public Task<UpdateAccountViewModel> GetAccountByIdAsyncForUpdate(string userId, int accountId);
        public Task<List<AccountViewModel>> GetListAccountsByNameAsync(string userId);
        public Task<List<AccountViewModel>> GetListDeleteAccountsByNameAsync(string userId);
        public Task<bool> IsAccountForUser(string username,int accountId);
        public Task<int> CreateAccount(CreateAccountViewModel accountVM);
        public Task<int> UpdateAccount(UpdateAccountViewModel accountVM,string userid);
        public Task<bool> DeleteAccountAsync(int accountId, string userId);
        public Task<bool> IncreaseAccountBalance(IncreaseBalanceDto balanceDto,string username);
        public Task<bool> IsCartNumberExist(string cartNumber);
        public Task<List<TransactionDto>> GetAccountTransactionsAsync(int accountId);
        public Task<List<TransactionDto>> GetUserTransactions(string userId);
        public Task<bool> Withdrawal(int accountId,string userId,decimal amount);
        public Task<bool> SaveAccountAddressForSendMoney(int accountId, string userId,string address="");
        public Task<ConfirmAddressAccountForDeleteDto> GetConfirmAccountDeleteInfo(int accountId,string userId);
        public Task<bool> ConfirmAccountDeleteInfo(int accountId,string userId);
        public Task<List<ConfirmAddressAccountForDeleteDto>> GetAccountDeleteInfo(string userId);
       



    }
}
