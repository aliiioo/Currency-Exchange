using Application.Contracts.Persistence;
using Application.Dtos.AccountDtos;
using Application.Statics;
using Currency_Exchange.Security;
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
        private readonly IOthersAccountServices _othersAccountServices;
        private readonly ICurrencyServices _currencyServices;

        public BankAccountController(ILogger<HomeController> logger, IAccountServices accountServices, IOthersAccountServices othersAccountServices, ICurrencyServices currencyServices)
        {
            _logger = logger;
            _accountServices = accountServices;
            _othersAccountServices = othersAccountServices;
            _currencyServices = currencyServices;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var bankAccounts = await _accountServices.GetListAccountsByNameAsync(User.GetUserId());
            return View(bankAccounts);
        }

        [HttpGet]
        public async Task<IActionResult> BankAccount(int accountId)
        {
            var bankAccount = await _accountServices.GetAccountByIdAsync(User.GetUserId(), accountId);
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

        [ServiceFilter(typeof(SanitizeInputFilter))]
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
                _logger.LogError($"Hacker {User.Identity?.Name}");
                var Error = "You are UnNormal ";
                return RedirectToAction("Error","Home",new{Error} );
            }
            var bankId=await _accountServices.CreateAccount(createAccountVM);
            if (bankId != 0) return RedirectToAction("Index", new { User.Identity?.Name });

            const string error = "Money is lower than 50$";
            return RedirectToAction("Error", "Home",new {error});
        }

        [HttpGet]
        public async Task<IActionResult> UpdateBankAccount(int accountId)
        {
            var account = await _accountServices.GetAccountByIdAsyncForUpdate(User.GetUserId(), accountId);
            return View(account);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(SanitizeInputFilter))]
        public async Task<IActionResult> UpdateBankAccount(UpdateAccountViewModel accountVM)
        {
            if (!ModelState.IsValid)
            {
                return View(accountVM);
            }
            var account = await _accountServices.GetAccountByIdAsync(User.GetUserId(), accountVM.AccountId);
            if (User.GetUserId() != accountVM.UserId || account.Balance != accountVM.Balance)
            {
                _logger.LogError($"Hacker {User.Identity?.Name}");
                var Error = "You are Hacker";
                return RedirectToAction("Error", "Home", new { Error });
            }
            await _accountServices.UpdateAccount(accountVM, accountVM.UserId);
            return RedirectToAction("Index", new { User.Identity?.Name });
        }

        public async Task<IActionResult> DeleteAccount(int accountId)
        {
            var account = await _accountServices.GetAccountByIdAsync(User.GetUserId(), accountId);
            if (account.Balance > 1)
            {
                return RedirectToAction("SendBalanceToAddressForDelete",new {accountId});
            }
            else
            {
                var result = await _accountServices.DeleteAccountAsync(accountId, User.GetUserId());
                await _accountServices.SaveAccountAddressForSendMoney(accountId, User.GetUserId(), "");
                if (result == false) return Unauthorized();
                return RedirectToAction("Index");
            }
        }

        public IActionResult SendBalanceToAddressForDelete(int accountId)
        {
            ViewBag.accountId = accountId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(SanitizeInputFilter))]
        public async Task<IActionResult> SendBalanceToAddressForDelete(AddressAccountForDeleteDto vmDto)
        {
            if (!ModelState.IsValid)
            {
                return View(vmDto);
            }
            var result= await _accountServices.SaveAccountAddressForSendMoney(vmDto.AccountId, User.GetUserId(),vmDto.Address);
            if (result == false) return Unauthorized();
            return RedirectToAction("ConfirmDelete" ,new {vmDto.AccountId});
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(int accountId)
        {
            var confirmAccountDelete = await _accountServices.GetConfirmAccountDeleteInfo(accountId, User.GetUserId());
            return View(confirmAccountDelete);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(SanitizeInputFilter))]
        public async Task<IActionResult> ConfirmDelete(ConfirmAddressAccountForDeleteDto vmdto)
        {
            if (!ModelState.IsValid)
            {
                return View(vmdto);
            }
            if (!vmdto.IsConfirm) return RedirectToAction("Index");
            var result = await _accountServices.DeleteAccountAsync(vmdto.AccountId, User.GetUserId());
            if (result == false) return RedirectToAction("Error", "Home");
            await _accountServices.ConfirmAccountDeleteInfo(vmdto.AccountId, User.GetUserId());
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult IncreaseBalance(int accountId, string accountCurrency)
        {
            var currency = _currencyServices.GetListCurrency().Result
                .Select(x => new SelectListItem { Value = x.CurrencyCode.ToString(), Text = x.CurrencyCode.ToString() }).ToList();
            currency.Insert(0, new SelectListItem { Value = "", Text = "انتحاب کنید" });
            ViewBag.currency = currency;
            ViewBag.accountCurrency = accountCurrency;
            ViewBag.accountId = accountId;
            return View();
        }
        [ServiceFilter(typeof(SanitizeInputFilter))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IncreaseBalance(IncreaseBalanceDto balanceDto)
        {
            if (!ModelState.IsValid)
            {
                return View(balanceDto);
            }
            var result = await _accountServices.IncreaseAccountBalance(balanceDto, User.GetUserId());
            if (result == false) return Unauthorized();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> AccountTransactions(int accountId)
        {
            var transactions = await _accountServices.GetUserAccountTransactionsAsync(accountId);
            ViewBag.accountId=accountId;
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
            ViewBag.accountId = accountId;
            return View();
        }

        [ServiceFilter(typeof(SanitizeInputFilter))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Withdrawal(WithdrawalDto withdrawalDto)
        {
            if (!ModelState.IsValid)
            {
                return View(withdrawalDto);
            }
            var result = await _accountServices.Withdrawal(withdrawalDto.AccountId, User.GetUserId(), withdrawalDto.Amount);
            if (result == false)
            {
                const string error = "Account must have more than 50$ for Transaction or Withdrawal";
                return RedirectToAction("Error", "Home", new { error });
            }
            return RedirectToAction("Index");
            
        }

        [HttpGet]
        public async Task<IActionResult> DeleteAccounts()
        {
            var bankAccounts = await _accountServices.GetListDeleteAccountsByNameAsync(User.GetUserId());
            return View(bankAccounts);
        }

    }
}
