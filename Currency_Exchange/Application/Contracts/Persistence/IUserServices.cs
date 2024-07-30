using Application.Dtos.ErrorsDtos;
using Application.Dtos.OthersAccountDtos;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Contracts.Persistence
{
    public interface IUserServices
    {
        public ResultDto ValidateModel(RegisterViewModel model);
        public ResultDto ValidateEmail(string email);
        public ResultDto ValidatePhoneNumber(string phone);
        public Task<ApplicationUser?> RegisterAsync(RegisterViewModel model);
        public Task ConfigureEmail(ApplicationUser user, string domain);
        public Task<ResultDto>  CheckEmailConfirmtion(LoginViewModel model);
        public Task<ResultDto> CheckLoginStatus(SignInResult userLogin);


    }
}
