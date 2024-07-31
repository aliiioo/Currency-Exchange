using Application.Contracts.Persistence;
using Application.Dtos.OthersAccountDto;
using Application.Statics;
using Currency_Exchange.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Currency_Exchange.Controllers
{
    [Authorize]
    public class OthersBankController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOthersAccountServices _othersAccountServices;
        private readonly ICurrencyServices _currencyServices;

        public OthersBankController(ILogger<HomeController> logger, IOthersAccountServices othersAccountServices, ICurrencyServices currencyServices)
        {
            _logger = logger;
            _othersAccountServices = othersAccountServices;
            _currencyServices = currencyServices;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var othersBank = await _othersAccountServices.GetListOthersAccountsByNameAsync(User.GetUserId());
            return View(othersBank);
        }


        [HttpGet]
        public async Task<IActionResult> OthersBank(int accountId)
        {
            var othersBank = await _othersAccountServices.GetOtherAccountByIdAsync(accountId,User.GetUserId());
            return View(othersBank);
        }


        [HttpGet]
        public IActionResult CreateOthersBankAccount()
        {
            var currency = _currencyServices.GetSelectListItemsCurrency();
            ViewBag.currency= currency;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(SanitizeInputFilter))]
        public async Task<IActionResult> CreateOthersBankAccount(CreateOtherAccountViewModel createAccountVM)
        {
            if (!ModelState.IsValid)
            {
                return View(createAccountVM);
            }
            if (createAccountVM.UserId != User.GetUserId())
            {
                _logger.LogError($"Unauthorized entry {User.Identity?.Name}");
                return RedirectToAction("Error","Home");
            }
            var account= await _othersAccountServices.CreateOthersAccountAsync(createAccountVM);
            if (account == 0)
            {
                const string error = "Don't Have Any Account With This Info";
                return RedirectToAction("Error", "Home",new {error});
            }
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> UpdateBankAccount(int accountId)
        {
            var account = await _othersAccountServices.GetOtherAccountByNameForUpdateAsync(accountId);
            return View(account);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(SanitizeInputFilter))]
        public async Task<IActionResult> UpdateBankAccount(UpdateOtherAccountViewModel accountVM)
        {
            if (!ModelState.IsValid)
            {
                return View(accountVM);
            }
            if (accountVM.UserId != User.GetUserId())
            {
                _logger.LogError($"Unauthorized entry {User.Identity?.Name}");
                return RedirectToAction("Error", "Home");
            }
            var account = await _othersAccountServices.UpdateOthersAccountAsync(accountVM,accountVM.UserId);
            if (account == 0)
            {
                const string error = "Don't Have Any Account With This Info";
                return RedirectToAction("Error", "Home", new { error });
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteOthersAccount(int accountId)
        {
            var result= await _othersAccountServices.DeleteOthersAccountAsync(accountId,User.GetUserId());
            if (result == false) return Unauthorized();
            return RedirectToAction("Index");
        }

        
    }
}
