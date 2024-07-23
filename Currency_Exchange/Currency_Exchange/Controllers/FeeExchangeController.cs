using System.Reflection.PortableExecutable;
using Application.Contracts.Persistence;
using Application.Dtos.CurrencyDtos;
using Currency_Exchange.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol;

namespace Currency_Exchange.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FeeExchangeController : Controller
    {
        private readonly ICurrencyServices _currencyServices;

        public FeeExchangeController(ICurrencyServices currencyServices)
        {
            _currencyServices = currencyServices;
        }


        [HttpGet]
        public IActionResult Create(int currentId, string currencyCode)
        {
            var currency = _currencyServices.GetListCurrency().Result
                .Select(x => new SelectListItem { Value = x.CurrencyCode.ToString(), Text = x.CurrencyCode.ToString() }).ToList();
            currency.Insert(0, new SelectListItem { Value = "", Text = "انتحاب کنید" });
            ViewBag.currency = currency;
            ViewBag.currentId = currentId;
            ViewBag.currencyCode = currencyCode;
            return View();
        }
        [HttpPost]
        [ServiceFilter(typeof(SanitizeInputFilter))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateFeeDtos Model)
        {
            if (!ModelState.IsValid)
            {
                var currency = _currencyServices.GetListCurrency().Result
                    .Select(x => new SelectListItem { Value = x.CurrencyCode.ToString(), Text = x.CurrencyCode.ToString() }).ToList();
                currency.Insert(0, new SelectListItem { Value = "", Text = "انتحاب کنید" });
                ViewBag.currency = currency;
                ViewBag.currentId = Model.CurrencyId;
                ViewBag.currencyCode = Model.FromCurrency;
                return View(Model);
            }
            var fee = await _currencyServices.CreateExchangeFeeToCurrency(Model);
            if (fee == 0)
            {
                return View(Model);
            }
            return RedirectToAction("index", "CurrencyAccounts");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int exchangeFeeId)
        {
            var exchangeFee = await _currencyServices.GetExchangeFeeCurrencyByIdAsync(exchangeFeeId);
            ViewBag.feeId = exchangeFeeId;
            return View(exchangeFee);
        }

        [ServiceFilter(typeof(SanitizeInputFilter))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int feeId, decimal feePrice)
        {
            if (feeId == 0 || feePrice == 0)
            {
                return View();
            }
            var transformFee = await _currencyServices.UpdateExchangeFeeToCurrency(feeId, feePrice);
            return RedirectToAction("index", "CurrencyAccounts");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int feeId, int currentId)
        {
            var result = await _currencyServices.DeleteExchangeFeeToCurrency(feeId, currentId);
            if (result)
            {
                return RedirectToAction("Index", "CurrencyAccounts");
            }
            return RedirectToAction("Error", "Home");
        }




    }
}
