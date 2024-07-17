using Application.Contracts.Persistence;
using Application.Dtos.TransactionDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using static System.Formats.Asn1.AsnWriter;

namespace Currency_Exchange.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProviderServices _providerServices;
        private readonly IAccountServices _accountServices;

        public TransactionController(ILogger<HomeController> logger, IProviderServices providerServices, IAccountServices accountServices)
        {
            _logger = logger;
            _providerServices = providerServices;
            _accountServices = accountServices;
        }

        public async Task<IActionResult> TransactionsList(int accountId)
        {
            var transactions=await _providerServices.GetListTransactions(accountId);
            return View(transactions);
        }


        public IActionResult TransactionAmount()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TransactionAmount(CreateTransactionDtos transactionDto)
        {
            if (!ModelState.IsValid)
            {
                return View(transactionDto);
            }
            if (User.Identity.Name != transactionDto.Username)
            {
                _logger.LogError($"Unauthorized entry {User.Identity.Name}");
                return Unauthorized();
            }

            var isToSelfAccount = await _accountServices.IsAccountForUser(User.Identity.Name, transactionDto.OthersAccountId);
            if (isToSelfAccount)
            {
                var result =
                    await _providerServices.TransformToSelfAccountCurrency(transactionDto, User.Identity.Name);
                if (result == false) return BadRequest();
            }
            else
            {
                var result = await _providerServices.TransformCurrency(transactionDto, User.Identity.Name);
                if (result == false) return BadRequest();
             
            }
            return RedirectToAction("");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmTransaction(int transactionId)
        {
            var transaction =await _providerServices.GetTransaction(transactionId);
            return View(transaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmTransaction(int transactionId,bool isConfirm=false)
        {
            using var safeScpoe= new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var result = await _providerServices.ConfirmTransaction(transactionId, User.Identity.Name, isConfirm);
                if (result == false)
                {
                    _logger.LogError($"Error in Transaction Unauthorized entry  {User.Identity.Name}");
                    return Unauthorized();
                }
                safeScpoe.Complete();
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
