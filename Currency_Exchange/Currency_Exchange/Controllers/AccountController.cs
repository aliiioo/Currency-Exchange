using Application.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Currency_Exchange.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMessageSender _messageSender;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(IMessageSender messageSender, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _messageSender = messageSender;
            _userManager = userManager;
            _signInManager = signInManager; 
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }




    }
}
