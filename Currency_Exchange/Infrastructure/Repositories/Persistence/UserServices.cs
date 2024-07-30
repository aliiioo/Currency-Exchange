using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Application.Contracts;
using Application.Contracts.Persistence;
using Application.Dtos.ErrorsDtos;
using Application.Dtos.OthersAccountDtos;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories.Persistence
{
    public class UserServices: IUserServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        // private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly IMessageSender _messageSender;

        public UserServices(UserManager<ApplicationUser> userManager, IMessageSender messageSender)
        {
            _userManager = userManager;
            _messageSender = messageSender;
        }


        public ResultDto ValidateModel(RegisterViewModel model)
        {
            var isValidPhoneNumber = ValidatePhoneNumber(model.Phone);
            var isValidEmail = ValidateEmail(model.Email);
            if (!isValidEmail.Result)
            {
                return isValidEmail;
            }
            return !isValidPhoneNumber.Result ? isValidPhoneNumber : new ResultDto();
        }

        public ResultDto ValidateEmail(string email)
        {
            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            var match = regex.Match(email);
            return match.Success ? new ResultDto() { Result = true } : new ResultDto() { Result = false, ErrorMessage = "Email is Not True Format" };
        }

        public ResultDto ValidatePhoneNumber(string phone)
        {
            var regex = new Regex(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}");
            var match = regex.Match(phone);
            return match.Success ? new ResultDto() { Result = true } : new ResultDto() { Result = false, ErrorMessage = "Phone Number is Not True Format" };
        }

        public async Task<ApplicationUser?> RegisterAsync(RegisterViewModel model)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, PhoneNumber = model.Phone, FullName = model.FullName };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddClaimAsync(user, new Claim("FullName", model.FullName));
                return user;
            }
            return null;
        }

        public async Task ConfigureEmail(ApplicationUser user,string domain)
        {
            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var url= $"{domain}/Account/CheckEmailConfirmtion?username={user.UserName}&token={emailConfirmationToken}";
            // var url = Url.Action("CheckEmailConfirmtion", "Account", new { username = user.UserName, token = emailConfirmationToken }, Request.Scheme);
            _messageSender.SendEmailAsync(user.Email, "Email confirmation", url);
        }

        public async Task<ResultDto> CheckEmailConfirmtion(LoginViewModel model)
        {
            var result = new ResultDto(); 
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user.EmailConfirmed == false)
            {
                result.ErrorMessage = "Email Not Confirm";
                return result;
            }
            return result;

        }

        public async Task<ResultDto> SignIn(LoginViewModel model)
        {
            var signInRes= new ResultDto();
            var isEmailConfirm = await CheckEmailConfirmtion(model);
            if (!isEmailConfirm.Result)
            {
                signInRes.ErrorMessage = "Email IS not confirmed";
                return signInRes;
            }
            return signInRes;
        }

        public async Task<ResultDto> CheckLoginStatus(SignInResult userLogin)
        {
            var result = new ResultDto();
            if (!userLogin.Succeeded)
            {
                result.ErrorMessage = userLogin.IsLockedOut ? "Your Account is Lock Try it After 10 Min" : "Username or Password is wrong!";
                return result;
            }
            result.Result = true;
            return result;
        }
    }
}
