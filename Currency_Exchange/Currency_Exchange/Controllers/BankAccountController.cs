using System.Diagnostics.CodeAnalysis;
using Application.Contracts.Persistence;
using Application.Dtos.AccountDtos;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Currency_Exchange.Controllers
{
    [Authorize]
    public class BankAccountController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAccountServices _accountServices;

        public BankAccountController(ILogger<HomeController> logger, IAccountServices accountServices)
        {
            _logger = logger;
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBankAccount(CreateAccountViewModel createAccountVM)
        {
            if (!ModelState.IsValid)
            {
                return View(createAccountVM);
            }
            if (User.Identity.Name!=createAccountVM.AccountName)
            {
                _logger.LogError($"Unauthorized entry {User.Identity.Name}");
                return Unauthorized();
            }
            await _accountServices.CreateAccount(createAccountVM);
            return RedirectToAction("Index", new { User.Identity.Name });
        }

        [HttpGet]
        public async Task<IActionResult> UpdateBankAccount(int accountId)
        {
            var account = await _accountServices.GetAccountByIdAsyncForUpdate(User.Identity.Name, accountId);
            return View(account);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBankAccount(UpdateAccountViewModel accountVM)
        {
            if (!ModelState.IsValid)
            {
                return View(accountVM);
            }
            if (User.Identity.Name != accountVM.AccountName)
            {
                _logger.LogError($"Unauthorized entry {User.Identity.Name}");
                return Unauthorized();
            }
            await _accountServices.UpdateAccount(accountVM);
            return RedirectToAction("Index", new { User.Identity.Name });
        }

        public async Task<IActionResult> DeleteAccount(int accountId)
        {
            var result= await _accountServices.DeleteAccountAsync(accountId,User.Identity.Name);
            if (result==false) return Unauthorized();
            return RedirectToAction("Index", new { User.Identity.Name });
        }

        [HttpGet]
        public IActionResult IncreaseBalance(int accountid)
        {
            ViewBag.accountid = accountid;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IncreaseBalance(IncreaseBalanceDto balanceDto)
        {
            if (!ModelState.IsValid)
            {
                return View(balanceDto);
            }
            var result=await _accountServices.IncreaseAccountBalance(balanceDto,User.Identity.Name);
            if (result==false) return Unauthorized();
            return RedirectToAction("Index", new { User.Identity.Name });
        }
    }
}
