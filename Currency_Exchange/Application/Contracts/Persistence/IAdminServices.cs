using Application.Dtos.AccountDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Task<bool> ActivateAccount(int accountId);
        public Task<bool> DeActivateAccount(int accountId);
    }
}
