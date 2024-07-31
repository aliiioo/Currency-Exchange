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
    public class FeeTransformController : Controller
    {

        private readonly ICurrencyServices _currencyServices;

        public FeeTransformController(ICurrencyServices currencyServices)
        {
            _currencyServices = currencyServices;
        }

        [HttpGet]
        public IActionResult Create(int currentId,string currencyCode)
        {
            var currency = _currencyServices.GetSelectListItemsCurrency();
            ViewBag.currency = currency;
            ViewBag.currentId = currentId;
            ViewBag.currencyCode=currencyCode;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(SanitizeInputFilter))]
        public async Task<IActionResult> Create(CreateFeeDtos Model)
        {
            if (!ModelState.IsValid|| string.IsNullOrEmpty(Model.ToCurrency))
            {
                var currency = _currencyServices.GetSelectListItemsCurrency();
                ViewBag.currency = currency;
                ViewBag.currentId = Model.CurrencyId;
                ViewBag.currencyCode = Model.FromCurrency;
                return View(Model);
            }
            var fee = await _currencyServices.CreateTransformFeeToCurrencyAsync(Model);
            return fee==0 ? RedirectToAction("Create",new { currentId =Model.CurrencyId, currencyCode=Model.FromCurrency}) : RedirectToAction("index", "CurrencyAccounts");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int transformFeeId)
        {
            ViewBag.transformFeeId = transformFeeId;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(SanitizeInputFilter))]
        public async Task<IActionResult> Update(int feeId, decimal feePrice)
        {
            if (feeId == 0 || feePrice == 0)
            {
                return View();
            }
            var transformFee = await _currencyServices.UpdateTransformFeeToCurrencyAsync(feeId,feePrice);
            const string error = "Price Must be in range next and previous";
            return transformFee==false ? RedirectToAction("Error", "Home",new {error}) : RedirectToAction("index", "CurrencyAccounts");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int feeId,int currentId)
        {
            var result = await _currencyServices.DeleteTransformFeeToCurrencyAsync(feeId,currentId);
            return result ? RedirectToAction("Index", "CurrencyAccounts") : RedirectToAction("Error","Home");
        }






    }
}
