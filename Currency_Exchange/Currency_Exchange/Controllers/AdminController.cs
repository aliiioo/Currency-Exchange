using Application.Contracts.Persistence;
using Application.Statics;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;

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
            var accounts = await _providerServices.GetListTransactionsAsync(accountId);
            return View(accounts);
        }

        public async Task<IActionResult> AllTransactions()
        {
            var accounts = await _providerServices.GetListTransactionsForAdminAsync();
            return View(accounts);
        }

        public async Task<IActionResult> DeletedAccounts()
        {
            var accounts = await _adminServices.GetDeletedAccountsForAdminAsync();
            return View(accounts);
        }

        public async Task<IActionResult> DisActiveAccounts()
        {
            var accounts = await _adminServices.GetDisActiveAccountsForAdminAsync();
            return View(accounts);
        }


        [HttpGet]
        public async Task<IActionResult> DeActivateAccount(int accountId)
        {
            var result = await _adminServices.DeActivateAccountAsync(accountId);
            return result ? RedirectToAction("Accounts") : RedirectToAction("Error", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> ActivateAccount(int accountId)
        {
            var result = await _adminServices.ActivateAccountAsync(accountId);
            return result == true ? RedirectToAction("Accounts") : RedirectToAction("Error", "Home");
        }

        public async Task<IActionResult> SearchAccountById(int accountId = 0)
        {
            var account = await _adminServices.GetAccountByIdForAdminAsync(accountId);
            ViewBag.accountId = accountId;
            return View(account);

        }
        public async Task<IActionResult> SearchAccountByCartNumber(string cartNumber)
        {
            if (!ValidateCartNumber.IsValidCardNumber(cartNumber))
            {
                const string error = "cart number is not valid";
               return RedirectToAction("Error", "Home",new{erorr=error});
            }
            var account = await _adminServices.GetAccountByCartNumberForAdminAsync(cartNumber);
            if (account!=null) ViewBag.accountId = account.AccountId;
            return View(account);
        }


        public async Task<IActionResult> AllDeletedAccountWithAddress()
        {
            var accounts =await _adminServices.GetAccountDeleteInfoForAdminAsync();
            return View(accounts);
        }


    }
}
