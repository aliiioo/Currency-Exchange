using Application.Contracts.Persistence;
using Application.Dtos.TransactionDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.Transactions;
using Application.Statics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using static System.Formats.Asn1.AsnWriter;

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
        public async Task<IActionResult> TransactionAmount(CreateTransactionDtos transactionDto)
        {
            
            if (!ModelState.IsValid)
            {
                return View(transactionDto);
            }
            if (!User.GetUserId().Equals(transactionDto.Username))
            {
                _logger.LogError($"Unauthorized entry {User.Identity.Name}");
                return Unauthorized();
            }
            transactionDto.OthersAccountId = int.Parse(transactionDto.OthersAccountIdAsString);
            var isToSelfAccount = await _accountServices.IsAccountForUser(User.GetUserId(), transactionDto.OthersAccountId);
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
        public async Task<IActionResult> ConfirmTransaction(int transactionId)
        {
            ViewBag.transactionId = transactionId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmTransaction(ConfirmTransactionDto confirmTransactionDto)
        {
            using var safeScope= new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var result = await _providerServices.ConfirmTransaction(confirmTransactionDto.TransactionId, User.GetUserId(), confirmTransactionDto.IsConfirm);
                if (result == false)
                {
                    _logger.LogError($"Error in Transaction Unauthorized entry  {User.Identity.Name}");
                    return Unauthorized();
                }
                safeScope.Complete();
                return RedirectToAction("Index", "BankAccount", new { User.Identity.Name });
            }
            catch (Exception e)
            {
                _logger.LogError($" More Request for Transaction {User.Identity.Name}");
                throw;
            }
           
        }


    }
}
