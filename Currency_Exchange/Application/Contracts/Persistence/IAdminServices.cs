using Application.Dtos.AccountDtos;

namespace Application.Contracts.Persistence
{
    public interface IAdminServices
    {
        public Task<List<AccountViewModel>> GetAccountsForAdminAsync();
        public Task<List<AccountViewModel>> GetDeletedAccountsForAdminAsync();
        public Task<List<AccountViewModel>> GetDisActiveAccountsForAdmin();
        public Task<AccountViewModel> GetAccountByIdForAdmin(int accountId);
        public Task<AccountViewModel> GetAccountByCartNumberForAdmin(string cartNumber);
        public Task<List<AccountViewModel>> GetUsersAccountsForAdmin(string email);
        public Task<List<DeleteAccountAddressDto>> GetAccountDeleteInfoForAdmin();
        public Task<bool> ActivateAccount(int accountId);
        public Task<bool> DeActivateAccount(int accountId);
    }
}
