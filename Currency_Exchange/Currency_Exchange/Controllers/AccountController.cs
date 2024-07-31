using Application.Contracts;
using Application.Dtos.OthersAccountDtos;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Application.Contracts.Persistence;

namespace Currency_Exchange.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserServices _userServices;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserServices userServices)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userServices = userServices;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var isValidate=_userServices.ValidateModel(model);
            if (!isValidate.IsSucceeded)
            {
                ModelState.AddModelError("", isValidate.Message);
                return View(model);
            }
            var user=await _userServices.RegisterAsync(model); 
            if (user !=null)
            {
                var domain = $"{Request.Scheme}://{Request.Host}";
                await _userServices.SendConfirmationEmail(user, domain);
                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult Login()
        {
            if (_signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (_signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid) return View(model);

            var isEmailConfirm= await _userServices.CheckEmailConfirmation(model);
            if (!isEmailConfirm.IsSucceeded)
            {
                ModelState.AddModelError("",isEmailConfirm.Message);
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, true);
            var loginResult= await _userServices.CheckLoginStatus(result);
            if (!loginResult.IsSucceeded)
            {
                ModelState.AddModelError("", "Error Login");
                return View(model);

            }
            return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userName, string token)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(token)) return NotFound();
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) return NotFound();
            var result = await _userManager.ConfirmEmailAsync(user, token);
            await _userManager.AddToRoleAsync(user, "Customer");
            return Content(result.Succeeded ? "Email Confirmed" : "(Error) Email Not Confirmed");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }






    }
}
