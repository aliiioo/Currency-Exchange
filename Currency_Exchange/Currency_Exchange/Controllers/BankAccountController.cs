using System.Diagnostics.CodeAnalysis;
using Application.Contracts.Persistence;
using Application.Dtos.AccountDtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Currency_Exchange.Controllers
{
    [Authorize]
    public class BankAccountController : Controller
    {
        private readonly IAccountServices _accountServices;

        public BankAccountController(IAccountServices accountServices)
        {
            _accountServices = accountServices;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string username)
        {
            var bankAccounts =await _accountServices.GetListAccountsByNameAsync(username);
            return View(bankAccounts);
        }

        [HttpGet]
        public async Task<IActionResult> BankAccount(string username,int accountId)
        {
            var bankAccount = await _accountServices.GetAccountByIdAsync(username,accountId);
            return View(bankAccount);
        }

        [HttpGet]
        public IActionResult CreateBankAccount()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateBankAccount(CreateAccountViewModel createAccountVM)
        {
            if (!ModelState.IsValid)
            {
                return View(createAccountVM);
            }
            await _accountServices.CreateAccount(createAccountVM);
            return RedirectToAction("Index", new { User.Identity.Name });
        }

        public async Task<IActionResult> UpdateBankAccount(int accountId)
        {
            var account = await _accountServices.GetAccountByIdAsyncForUpdate(User.Identity.Name, accountId);
            return View(account);

        }

        [HttpPost]
        public async Task<IActionResult> UpdateBankAccount(UpdateAccountViewModel accountVM)
        {
            if (!ModelState.IsValid)
            {
                return View(accountVM);
            }
            await _accountServices.UpdateAccount(accountVM);
            return RedirectToAction("Index", new { User.Identity.Name });
        }

        public async Task<IActionResult> DeleteAccount(int accountId)
        {
            await _accountServices.DeleteAccountAsync(accountId);
            return RedirectToAction("Index", new { User.Identity.Name });
        }

        [HttpGet]
        public IActionResult IncreaseBalance(int accountid)
        {
            ViewBag.accountid = accountid;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> IncreaseBalance(IncreaseBalanceDto balanceDto)
        {
            if (!ModelState.IsValid)
            {
                return View(balanceDto);
            }

            await _accountServices.IncreaseAccountBalance(balanceDto);
            return RedirectToAction("Index", new { User.Identity.Name });
        }
    }
}
