using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Application.Contracts.Persistence;
using Application.Dtos.AccountDtos;
using Application.Statics;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Currency_Exchange.Controllers
{
    [Authorize]
    public class BankAccountController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAccountServices _accountServices;
        private readonly ICurrencyServices _currencyServices;

        public BankAccountController(ILogger<HomeController> logger, IAccountServices accountServices, ICurrencyServices currencyServices)
        {
            _logger = logger;
            _accountServices = accountServices;
            _currencyServices = currencyServices;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var bankAccounts =await _accountServices.GetListAccountsByNameAsync(User.GetUserId());
            return View(bankAccounts);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteAccounts()
        {
            var bankAccounts = await _accountServices.GetListAccountsByNameAsync(User.GetUserId());
            return View(bankAccounts);
        }

        [HttpGet]
        public async Task<IActionResult> BankAccount(int accountId)
        {
            var bankAccount = await _accountServices.GetAccountByIdAsync(User.GetUserId(),accountId);
            return View(bankAccount);
        }


        [HttpGet]
        public IActionResult CreateBankAccount()
        {
            var currency = _currencyServices.GetListCurrency().Result
                .Select(x => new SelectListItem { Value = x.CurrencyCode.ToString(), Text = x.CurrencyCode.ToString() }).ToList();
            currency.Insert(0, new SelectListItem { Value = "", Text = "انتحاب کنید" });
            ViewBag.currency = currency;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBankAccount(CreateAccountViewModel createAccountVM)
        {
            if (!ModelState.IsValid)
            {
                var currency = _currencyServices.GetListCurrency().Result
                    .Select(x => new SelectListItem { Value = x.CurrencyCode.ToString(), Text = x.CurrencyCode.ToString() }).ToList();
                currency.Insert(0, new SelectListItem { Value = "", Text = "انتحاب کنید" });
                ViewBag.currency = currency;
                return View(createAccountVM);
            }
            if (!User.GetUserId().Equals(createAccountVM.UserId))
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
            var account = await _accountServices.GetAccountByIdAsyncForUpdate(User.GetUserId(), accountId);
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
            await _accountServices.UpdateAccount(accountVM,accountVM.UserId);
            return RedirectToAction("Index", new { User.Identity.Name });
        }

        public async Task<IActionResult> DeleteAccount(int accountId)
        {
            var result= await _accountServices.DeleteAccountAsync(accountId,User.GetUserId());
            if (result==false) return Unauthorized();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult IncreaseBalance(int accountId,string accountCurrency)
        {
            var currency = _currencyServices.GetListCurrency().Result
                .Select(x => new SelectListItem { Value = x.CurrencyCode.ToString(), Text = x.CurrencyCode.ToString() }).ToList();
            currency.Insert(0, new SelectListItem { Value = "", Text = "انتحاب کنید" });
            ViewBag.currency = currency;
            ViewBag.accountCurrency = accountCurrency;
            ViewBag.accountId = accountId;
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
            var result=await _accountServices.IncreaseAccountBalance(balanceDto,User.GetUserId());
            if (result==false) return Unauthorized();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> AccountTransactions(int accountId)
        {
            var transactions =await _accountServices.GetAccountTransactionsAsync(accountId);
            return View(transactions);
        }

        public async Task<IActionResult> UserTransactions()
        {
            var transactions = await _accountServices.GetUserTransactions(User.GetUserId());
            return View(transactions);
        }

        [HttpGet]
        public async Task<IActionResult> Withdrawal(int accountId)
        {
            ViewBag.accountId=accountId;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Withdrawal(WithdrawalDto withdrawalDto)
        {
            if (!ModelState.IsValid)
            {
                return View(withdrawalDto);
            }
            var result= await _accountServices.Withdrawal(withdrawalDto.AccountId, User.GetUserId(), withdrawalDto.Amount);
            if (result==false) return Unauthorized();
            return RedirectToAction("Index");
        }




    }
}
