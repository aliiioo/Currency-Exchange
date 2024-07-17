using Application.Contracts.Persistence;
using Application.Dtos.AccountDtos;
using Application.Dtos.OthersAccountDto;
using Infrastructure.Repositories.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Core.Infrastructure;

namespace Currency_Exchange.Controllers
{
    [Authorize]
    public class OthersBankController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOthersAccountServices _othersAccountServices;

        public OthersBankController(IOthersAccountServices othersAccountServices)
        {
            _othersAccountServices = othersAccountServices;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string username)
        {
            var othersBank = await _othersAccountServices.GetListOthersAccountsByNameAsync(username);
            return View(othersBank);
        }

        [HttpGet]
        public async Task<IActionResult> OthersBank(int accountId)
        {
            var othersBank = await _othersAccountServices.GetOtherAccountByIdAsync(accountId,User.Identity.Name);
            return View(othersBank);
        }


        [HttpGet]
        public IActionResult CreateOthersBankAccount()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOthersBankAccount(CreateOtherAccountViewModel createAccountVM)
        {
            if (!ModelState.IsValid)
            {
                return View(createAccountVM);
            }
            if (createAccountVM.UserId != User.Identity.Name)
            {
                _logger.LogError($"Unauthorized entry {User.Identity.Name}");
                return Unauthorized();
            }
            await _othersAccountServices.CreateOthersAccountAsync(createAccountVM);
            return RedirectToAction("Index", new { User.Identity.Name });
        }


        public async Task<IActionResult> UpdateBankAccount(int accountId)
        {
            var account = await _othersAccountServices.GetOtherAccountByNameForUpdateAsync(accountId);
            return View(account);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBankAccount(UpdateOtherAccountViewModel accountVM)
        {
            if (!ModelState.IsValid)
            {
                return View(accountVM);
            }
            if (accountVM.UserId != User.Identity.Name)
            {
                _logger.LogError($"Unauthorized entry {User.Identity.Name}");
                return Unauthorized();
            }
            await _othersAccountServices.UpdateOthersAccountAsync(accountVM);
            return RedirectToAction("Index", new { User.Identity.Name });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteOthersAccount(int accountId)
        {
            var result= await _othersAccountServices.DeleteOthersAccountAsync(accountId,User.Identity.Name);
            if (result == false) return Unauthorized();
            return RedirectToAction("Index", new { User.Identity.Name });
        }












    }
}
