using Application.Contracts.Persistence;
using Application.Dtos.CurrencyDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NuGet.Packaging.Core;
using NuGet.Protocol;

namespace Currency_Exchange.Controllers
{
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

        [HttpGet]
        public IActionResult CreateCurrency()
        {
            return View();
        }

        [HttpPost]
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

            return RedirectToAction("CreateCurrency", "CurrencyAccounts");
        }

        [HttpGet]
        public async Task<IActionResult> DetailCurrencyExchangeFee(int currencyId)
        {
            var currencyExchangeFee = await _currencyServices.GetCurrencyExchangeFeeAsync(currencyId);
            return View(currencyExchangeFee);
        }

        [HttpGet]
        public async Task<IActionResult> DetailCurrencyTransformFee(int currencyId)
        {
            var currencyTransformFee = await _currencyServices.GetCurrencyTransformFeeAsync(currencyId);
            return View(currencyTransformFee);
        }

        [HttpGet]
        public async Task<IActionResult> DetailCurrencyTransformRate(int currencyId)
        {
            var currencyRates = await _currencyServices.GetCurrencyRatesAsync(currencyId);
            return View(currencyRates);
        }

    }
}
