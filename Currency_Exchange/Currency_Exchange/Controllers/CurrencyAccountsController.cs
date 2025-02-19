﻿using System.Reflection.Metadata.Ecma335;
using Application.Contracts.Persistence;
using Application.Dtos.CurrencyDtos;
using Currency_Exchange.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Currency_Exchange.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CurrencyAccountsController : Controller
    {
        private readonly ICurrencyServices _currencyServices;

        public CurrencyAccountsController(ICurrencyServices currencyServices)
        {
            _currencyServices = currencyServices;   
        }
        public async Task<IActionResult> Index()
        {
            var listCurrency =await _currencyServices.GetListCurrencyAsync();
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
            if (!ModelState.IsValid)
            {
                return View(model: Model);
            }
            var result= await _currencyServices.CreateCurrencyAsync(Model);
            if (!result.IsSucceeded)
            {
                return RedirectToAction("Error", "Home", new { error = result.Message });
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
