using Application.Contracts.Persistence;
using Application.Dtos.CurrencyDtos;
using Currency_Exchange.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NuGet.Packaging.Core;
using NuGet.Protocol;

namespace Currency_Exchange.Controllers
{
    [Authorize]
    public class CurrencyAccountsController : Controller
    {
        private readonly ICurrencyServices _currencyServices;

        public CurrencyAccountsController(ICurrencyServices currencyServices)
        {
            _currencyServices = currencyServices;   
        }
        public async Task<IActionResult> Index()
        {
            var listCurrency =await _currencyServices.GetListCurrency();
            return View(listCurrency);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreateCurrency()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ServiceFilter(typeof(SanitizeInputFilter))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCurrency(CurrencyDto Model)
        {
            if (!TryValidateModel(model:Model))
            {
                return View(model: Model);
            }
            var result= await _currencyServices.CreateCurrency(Model);
            if (result==0)
            {
                return View(model: Model);
            }

            return RedirectToAction("Index", "CurrencyAccounts");
        }

        [HttpGet]
        public async Task<IActionResult> DetailCurrencyExchangeFee(int currencyId,string currentCode)
        {
            var currencyExchangeFee = await _currencyServices.GetCurrencyExchangeFeeAsync(currencyId);
            ViewBag.currentCode=currentCode;
            ViewBag.currencyId = currencyId;
            return View(currencyExchangeFee);
        }

        [HttpGet]
        public async Task<IActionResult> DetailCurrencyTransformFee(int currencyId, string currentCode)
        {
            var currencyTransformFee = await _currencyServices.GetCurrencyTransformFeeAsync(currencyId);
            ViewBag.currentCode = currentCode;
            ViewBag.currencyId = currencyId;
            return View(currencyTransformFee);
        }

        [HttpGet]
        public async Task<IActionResult> DetailCurrencyTransformRate(int currencyId, string currentCode)
        {
            var currencyRates = await _currencyServices.GetCurrencyRatesAsync(currencyId);
            ViewBag.currentCode = currentCode;
            ViewBag.currencyId=currencyId;
            return View(currencyRates);
        }

    }
}
