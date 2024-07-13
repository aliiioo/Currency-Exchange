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
        public Task<int> CreateOthersAccountAsync(CreateOtherAccountViewModel othersAccountViewModel);
        public Task<int> DeleteOthersAccountAsync( int accountId);
        public Task<int> UpdateOthersAccountAsync(UpdateOtherAccountViewModel otherAccountViewModel);
        public Task<int> GetOtherAccountByNameAsync(int accountId);






    }
}
