using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.AccountDtos;
using Domain.Entities;

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
        public Task<int> UpdateAccount(UpdateAccountViewModel accountVM);
        public Task<bool> DeleteAccountAsync(int accountId, string userId);
        public Task<bool> IncreaseAccountBalance(IncreaseBalanceDto balanceDto,string username);
        public Task<bool> IsCartNumberExist(string cartNumber);


    }
}
