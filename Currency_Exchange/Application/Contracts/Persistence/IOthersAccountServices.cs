using Application.Dtos.OthersAccountDto;

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
