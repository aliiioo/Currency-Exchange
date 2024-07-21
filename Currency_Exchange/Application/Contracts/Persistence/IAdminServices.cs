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

        public Task<bool> ActiveAccount(int accountId);
        public Task<bool> DisActiveAccount(int accountId);
    }
}
