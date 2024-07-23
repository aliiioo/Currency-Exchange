using Application.Contracts;
using Application.Dtos.OthersAccountDtos;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Currency_Exchange.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMessageSender _messageSender;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(IMessageSender messageSender, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _messageSender = messageSender;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager; 
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
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email ,PhoneNumber = model.Phone,FullName = model.FullName};
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var emailConfirmationToken =
                        await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var emailMessage = Url.Action("ConfirmEmail", "Account",
                        new { username = user.UserName, token = emailConfirmationToken },
                            Request.Scheme);
                    if (emailMessage != null)
                        _messageSender.SendEmailAsync(model.Email, "Email confirmation", emailMessage);

                    if (!await _roleManager.RoleExistsAsync("Customer"))
                    {
                        RedirectToAction("Error", "Home");
                    }
                    await _userManager.AddToRoleAsync(user, "Customer");
                    await _userManager.AddClaimAsync(user, new Claim("FullName", model.FullName));
                    return RedirectToAction("Login", "Account");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            ModelState.AddModelError(string.Empty, "Invalid Inputs");
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
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user.EmailConfirmed == false)
                {
                    ModelState.AddModelError("", "Email Not Confirm");
                    return View(model);
                }
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email, model.Password, model.RememberMe,true);
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "You Are Lock");
                    return View(model);
                }
                if (result.Succeeded)
                {

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Wrong Arrive");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userName, string token)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(token))
                return NotFound();
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) return NotFound();
            var result = await _userManager.ConfirmEmailAsync(user, token);

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
