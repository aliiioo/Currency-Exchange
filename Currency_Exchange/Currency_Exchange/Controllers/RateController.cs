﻿using Application.Contracts.Persistence;
using Application.Dtos.CurrencyDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Currency_Exchange.Controllers
{
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
            List<SelectListItem> currency = _currencyServices.GetListCurrency().Result
                .Select(x => new SelectListItem { Value = x.CurrencyCode.ToString(), Text = x.CurrencyCode.ToString() }).ToList();
            currency.Insert(0, new SelectListItem { Value = "", Text = "انتحاب کنید" });
            ViewBag.currency=currency;
            ViewBag.currentId=currentId;
            ViewBag.currencyCode=currencyCode;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RateDtos rateDto)
        {
            if (!ModelState.IsValid)
            {
                List<SelectListItem> currency = _currencyServices.GetListCurrency().Result
                    .Select(x => new SelectListItem { Value = x.CurrencyCode.ToString(), Text = x.CurrencyCode.ToString() }).ToList();
                currency.Insert(0, new SelectListItem { Value = "", Text = "انتحاب کنید" });
                ViewBag.currency = currency;
                return View(rateDto);
            }
            var rate = await _currencyServices.CreateExchangeRateCurrency(rateDto);
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
        public async Task<IActionResult> Update(UpdateRateDtos rateDto)
        {
            if (!ModelState.IsValid)
            {
                return View(rateDto);
            }
            var transformFee = await _currencyServices.UpdateExchangeRateToCurrency(rateDto);
            return RedirectToAction("index", "CurrencyAccounts");
        }



    }
}
