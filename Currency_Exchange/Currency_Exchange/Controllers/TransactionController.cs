using Application.Contracts.Persistence;
using Application.Dtos.TransactionDtos;
using Application.Statics;
using Currency_Exchange.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Transactions;

namespace Currency_Exchange.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProviderServices _providerServices;
        private readonly IAccountServices _accountServices;
        private readonly ICurrencyServices _currencyServices;
        private readonly IOthersAccountServices _othersAccountServices;

        public TransactionController(ILogger<HomeController> logger, IProviderServices providerServices, IAccountServices accountServices, ICurrencyServices currencyServices, IOthersAccountServices othersAccountServices)
        {
            _logger = logger;
            _providerServices = providerServices;
            _accountServices = accountServices;
            _currencyServices = currencyServices;
            _othersAccountServices = othersAccountServices;
        }
        public async Task<IActionResult> TransactionsList(int accountId)
        {
            var transactions=await _providerServices.GetListTransactions(accountId);
            return View(transactions);
        }

        [HttpGet]
        public IActionResult TransactionAmount(int accountId,string fromCurrency)
        {
            var othersAccount= _othersAccountServices.GetListOthersAccountsByNameAsync(User.GetUserId()).Result
                .Select(x => new SelectListItem { Value = x.AccountId.ToString(), Text = x.CartNumber+"-"+x.AccountName.ToString() }).ToList();
            othersAccount.AddRange(_accountServices.GetListAccountsByNameAsync(User.GetUserId()).Result
                .Select(x => new SelectListItem { Value = x.AccountId.ToString(), Text = x.CartNumber + "-" + x.AccountName.ToString() }).ToList());
            othersAccount.Insert(0, new SelectListItem { Value = "", Text = "انتحاب کنید" });
            ViewBag.othersAccount = othersAccount;
            ViewBag.fromCurrency = fromCurrency;
            ViewBag.accountId=accountId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(SanitizeInputFilter))]
        public async Task<IActionResult> TransactionAmount(CreateTransactionDtos transactionDto)
        {
            
            if (!ModelState.IsValid)
            {
                return View(transactionDto);
            }
            if (!User.GetUserId().Equals(transactionDto.UserId))
            {
                var error = $"Hacker => {User.Identity?.Name}";
                _logger.LogError(error);
                return RedirectToAction("Error", "Home",new {error});
            }
            if (transactionDto.SelfAccountId.Equals(int.Parse(transactionDto.OthersAccountIdAsString)))
            {
                var error = "Transform to Your Current Account Is Not Allow";
                return RedirectToAction("Error", "Home", new { error });
            }
            
            var isToSelfAccount = await _accountServices.IsAccountForUser(User.GetUserId(), int.Parse(transactionDto.OthersAccountIdAsString));
            if (isToSelfAccount)
            {
                var transactionId =
                    await _providerServices.TransformToSelfAccountCurrency(transactionDto, User.GetUserId());
                if (transactionId == 0) return BadRequest();
                return RedirectToAction("ConfirmTransaction", new {transactionId });
            }
            else
            {
                var transactionId = await _providerServices.TransformCurrency(transactionDto, User.GetUserId());
                if (transactionId == 0) return BadRequest();
                return RedirectToAction("ConfirmTransaction", new { transactionId });
            }
        }

        [HttpGet]
        public async Task <IActionResult> ConfirmTransaction(int transactionId)
        {
            var transaction = await _providerServices.GetConfirmTransaction(transactionId,User.GetUserId());
            return View(transaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(SanitizeInputFilter))]
        public async Task<IActionResult> ConfirmTransaction(ConfirmTransactionDto confirmTransactionDto)
        {
            if (!ModelState.IsValid)
            {
                return View(confirmTransactionDto);
            }
            using var safeScope= new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var result = await _providerServices.ConfirmTransaction(confirmTransactionDto.TransactionId, User.GetUserId(), confirmTransactionDto.IsConfirm);
                if (result == false)
                {
                    var error = $" Transaction Wrong May it timeOut {User.Identity?.Name}";
                    _logger.LogError(error);
                    return RedirectToAction("Error", "Home", new { error });
                }
                safeScope.Complete();
                return RedirectToAction("TransactionDetail", new { confirmTransactionDto.TransactionId});
            }
            catch (Exception e)
            {
                var error = $" More Request for Transaction {User.Identity?.Name} for {e}";
                _logger.LogError(error);
                return RedirectToAction("Error", "Home",new { Error = error});

            }
        }

        [HttpGet]
        public async Task<IActionResult> TransactionDetail(int transactionId)
        {
            var transaction =await _providerServices.GetConfirmTransaction(transactionId,userId:User.GetUserId());
            return View(transaction);
        }



    }
}
