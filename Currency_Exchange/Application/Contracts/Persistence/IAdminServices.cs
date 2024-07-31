using Application.Dtos.AccountDtos;

namespace Application.Contracts.Persistence
{
    public interface IAdminServices
    {
        public Task<List<AccountViewModel>> GetAccountsForAdminAsync();
        public Task<List<AccountViewModel>> GetDeletedAccountsForAdminAsync();
        public Task<List<AccountViewModel>> GetDisActiveAccountsForAdminAsync();
        public Task<AccountViewModel> GetAccountByIdForAdminAsync(int accountId);
        public Task<AccountViewModel> GetAccountByCartNumberForAdminAsync(string cartNumber);
        public Task<List<AccountViewModel>> GetUsersAccountsForAdminAsync(string email);
        public Task<List<DeleteAccountAddressDto>> GetAccountDeleteInfoForAdminAsync();
        public Task<bool> ActivateAccountAsync(int accountId);
        public Task<bool> DeActivateAccountAsync(int accountId);
       

    }
}
