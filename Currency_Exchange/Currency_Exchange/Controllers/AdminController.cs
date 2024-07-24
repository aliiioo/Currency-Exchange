using Application.Contracts.Persistence;
using Application.Statics;
using Humanizer;
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
            return result ? RedirectToAction("Accounts") : RedirectToAction("Error", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> ActivateAccount(int accountId)
        {
            var result = await _adminServices.ActivateAccount(accountId);
            return result == true ? RedirectToAction("Accounts") : RedirectToAction("Error", "Home");
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
                var error = "cart number is not valid";
               return RedirectToAction("Error", "Home",new{error});
            }
            var account = await _adminServices.GetAccountByCartNumberForAdmin(cartNumber);
            if (account!=null)
            {
                ViewBag.accountId = account.AccountId;
            }
           
            return View(account);
        }


        public async Task<IActionResult> AllDeletedAccountWithAddress()
        {
            var accounts =await _adminServices.GetAccountDeleteInfoForAdmin();
            return View(accounts);
        }


    }
}
