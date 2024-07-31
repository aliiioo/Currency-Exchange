using Application.Contracts.Persistence;
using Application.Dtos.CurrencyDtos;
using Currency_Exchange.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Currency_Exchange.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RateController : Controller
    {
        private readonly ICurrencyServices _currencyServices;

        public RateController(ICurrencyServices currencyServices)
        {
            _currencyServices = currencyServices;   
        }

        [HttpGet]
        public IActionResult Create(int currentId,string currencyCode)
        {
            var currency = _currencyServices.GetSelectListItemsCurrency();
            ViewBag.currency=currency;
            ViewBag.currentId=currentId;
            ViewBag.currencyCode=currencyCode;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(SanitizeInputFilter))]
        public async Task<IActionResult> Create(RateDtos rateDto)
        {
            if (!ModelState.IsValid||string.IsNullOrEmpty(rateDto.ToCurrency))
            {
                var currency = _currencyServices.GetSelectListItemsCurrency();
                ViewBag.currency = currency;
                return View(rateDto);
            }
            var rate = await _currencyServices.CreateExchangeRateCurrencyAsync(rateDto);
            if (rate == 0)
            {
                return View(rateDto);
            }
            return RedirectToAction("index", "CurrencyAccounts");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int rateId)
        {
            var exchangeRate = await _currencyServices.GetCurrencyRateByIdAsync(rateId);
            return View(exchangeRate);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(SanitizeInputFilter))]
        public async Task<IActionResult> Update(UpdateRateDtos rateDto)
        {
            if (!ModelState.IsValid)
            {
                return View(rateDto);
            }
            var transformFee = await _currencyServices.UpdateExchangeRateToCurrencyAsync(rateDto);
            return transformFee == false? RedirectToAction("Error", "Home"): RedirectToAction("index", "CurrencyAccounts");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int feeId)
        {
            var result = await _currencyServices.DeleteExchangeRateCurrencyAsync(feeId);
            return result ? RedirectToAction("Index", "CurrencyAccounts") : RedirectToAction("Error", "Home");
        }


    }
}
