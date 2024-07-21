using Application.Contracts.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Currency_Exchange.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminServices _adminServices;

        public AdminController(IAdminServices adminServices)
        {
            _adminServices = adminServices;
        }

        public async Task<IActionResult> Accounts()
        {
            var accounts =await _adminServices.GetAccountsForAdminAsync();
            return View(accounts);
        }

        public async Task<IActionResult> DeletedAccounts()
        {
            var accounts = await _adminServices.GetDeletedAccountsForAdminAsync();
            return View(accounts);
        }

        public async Task<IActionResult> DisActiveAccounts()
        {
            var accounts = await _adminServices.GetDisActiveAccountsForAdmin();
            return View(accounts);
        }

        [HttpGet]
        public async Task<IActionResult> DeActivateAccount(int accountId)
        {
            var result = await _adminServices.DeActivateAccount(accountId);
            if (result==true)
            {
                return RedirectToAction("Accounts");
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> ActivateAccount(int accountId)
        {
            var result = await _adminServices.ActivateAccount(accountId);
            if (result == true)
            {
                return RedirectToAction("Accounts");
            }
            return NotFound();
        }


    }
}
