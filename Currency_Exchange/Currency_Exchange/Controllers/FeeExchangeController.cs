using Application.Contracts.Persistence;
using Application.Dtos.CurrencyDtos;
using Currency_Exchange.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            var currency = _currencyServices.GetSelectListItemsCurrency();
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
            if (!ModelState.IsValid||string.IsNullOrEmpty(Model.ToCurrency))
            {
                var currency = _currencyServices.GetSelectListItemsCurrency();
                ViewBag.currency = currency;
                ViewBag.currentId = Model.CurrencyId;
                ViewBag.currencyCode = Model.FromCurrency;
                return View(Model);
            }
            var fee = await _currencyServices.CreateExchangeFeeToCurrencyAsync(Model);
            if (fee == 0)
            {
                var currency = _currencyServices.GetSelectListItemsCurrency();
                ViewBag.currency = currency;
                ViewBag.currentId = Model.CurrencyId;
                ViewBag.currencyCode = Model.FromCurrency;
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
            var transformFee = await _currencyServices.UpdateExchangeFeeToCurrencyAsync(feeId, feePrice);
            const string error = "Price Must be in range next and previous";
            return transformFee==false ? RedirectToAction("Error", "Home",new{error}) : RedirectToAction("index", "CurrencyAccounts");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int feeId, int currentId)
        {
            var result = await _currencyServices.DeleteExchangeFeeToCurrencyAsync(feeId, currentId);
            return result ? RedirectToAction("Index", "CurrencyAccounts") : RedirectToAction("Error", "Home");
        }




    }
}
