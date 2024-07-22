using Application.Contracts.Persistence;
using Application.Statics;
using Infrastructure.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Currency_Exchange.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IProviderServices _providerServices;
        private readonly IAdminServices _adminServices;
       

        public AdminController(IProviderServices providerServices, IAdminServices adminServices)
        {
            _providerServices = providerServices;
            _adminServices = adminServices;
        }

        public async Task<IActionResult> Accounts()
        {
            var accounts =await _adminServices.GetAccountsForAdminAsync();
            return View(accounts);
        }

        public async Task<IActionResult> AccountTransactions(int accountId)
        {
            var accounts = await _providerServices.GetListTransactions(accountId);
            return View(accounts);
        }

        public async Task<IActionResult> AllTransactions()
        {
            var accounts = await _providerServices.GetListTransactionsForAdmin();
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

        public async Task<IActionResult> SearchAccountById(int accountId = 0)
        {
            var account = await _adminServices.GetAccountByIdForAdmin(accountId);
            ViewBag.accountId = accountId;
            return View(account);

        }
        public async Task<IActionResult> SearchAccountByCartNumber(string cartNumber)
        {
            if (!ValidateCartNumber.IsValidCardNumber(cartNumber))
            {
               return RedirectToAction("Error", "Home");
            }
            var account = await _adminServices.GetAccountByCartNumberForAdmin(cartNumber);
            if (account==null) return RedirectToAction("Error", "Home");
            ViewBag.accountId = account.AccountId;
            return View(account);
        }



    }
}
