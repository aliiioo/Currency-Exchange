using Application.Contracts.Persistence;
using Application.Dtos.CurrencyDtos;
using AutoMapper.Configuration.Annotations;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NuGet.Protocol;

namespace Currency_Exchange.Controllers
{
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
            List<SelectListItem> currency = _currencyServices.GetListCurrency().Result
                .Select(x => new SelectListItem { Value = x.CurrencyCode.ToString(), Text = x.CurrencyCode.ToString() }).ToList();
            currency.Insert(0, new SelectListItem { Value= "", Text = "انتحاب کنید" });
            ViewBag.currency = currency;
            ViewBag.currentId = currentId;
            ViewBag.currencyCode=currencyCode;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateFeeDtos Model)
        {
            if (!ModelState.IsValid)
            {
                List<SelectListItem> currency = _currencyServices.GetListCurrency().Result
                    .Select(x => new SelectListItem { Value = x.CurrencyCode.ToString(), Text = x.CurrencyCode.ToString() }).ToList();
                currency.Insert(0, new SelectListItem { Value = "", Text = "انتحاب کنید" });
                ViewBag.currency = currency;
                ViewBag.currentId = Model.CurrencyId;
                ViewBag.currencyCode = Model.FromCurrency;
                return View(Model);
            }
            var fee = await _currencyServices.CreateTransformFeeToCurrency(Model);
            if (fee==0)
            {
                return RedirectToAction("Create",new { currentId =Model.CurrencyId, currencyCode=Model.FromCurrency});
            }
            return RedirectToAction("index", "CurrencyAccounts");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int transformFeeId)
        {
            // var transformFee = await _currencyServices.GetTransformFeeCurrencyByIdAsync(transformFeeId);
            ViewBag.transformFeeId = transformFeeId;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int feeId, decimal feePrice)
        {
            if (feeId == 0 || feePrice == 0)
            {
                return View();
            }
            var transformFee = await _currencyServices.UpdateTransformFeeToCurrency(feeId,feePrice);
            return RedirectToAction("index", "CurrencyAccounts");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int feeId,int currentId)
        {
            var result = await _currencyServices.DeleteTransformFeeToCurrency(feeId,currentId);
            if (result)
            {
                return RedirectToAction("Index", "CurrencyAccounts");
            }
            return RedirectToAction("Error","Home");
        }






    }
}
