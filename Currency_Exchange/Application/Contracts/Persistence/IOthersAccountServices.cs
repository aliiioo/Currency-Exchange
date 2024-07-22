using Application.Dtos.OthersAccountDto;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Persistence
{
    public interface IOthersAccountServices
    {
        public Task<List<OtherAccountViewModel>> GetListOthersAccountsByNameAsync(string username);
        public Task<OtherAccountViewModel> GetOtherAccountByIdAsync(int accountId,string username);
        public Task<UpdateOtherAccountViewModel> GetOtherAccountByNameForUpdateAsync(int accountId);
        public Task<int> CreateOthersAccountAsync(CreateOtherAccountViewModel othersAccountViewModel);
        public Task<bool> DeleteOthersAccountAsync( int accountId,string userId);
        public Task<int> UpdateOthersAccountAsync(UpdateOtherAccountViewModel otherAccountViewModel,string userId);
        public Task<bool> IsAccountForOthers(string username,int accountId);







    }
}
