using Application.Contracts.Persistence;
using Application.Dtos.CurrencyDtos;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateFeeDtos Model)
        {
            if (!ModelState.IsValid)
            {
                return View(Model);
            }
            var fee = await _currencyServices.CreateTransformFeeToCurrency(Model);
            if (fee==0)
            {
                return View(Model);
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Update(int transformFeeId)
        {
            var transformFee = await _currencyServices.GetTransformFeeCurrencyByIdAsync(transformFeeId);
            return View(transformFee);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateFeeDtos feeDtos)
        {
            if (!ModelState.IsValid)
            {
                return View(feeDtos);
            }
            var transformFee = await _currencyServices.UpdateTransformFeeToCurrency(feeDtos);
            return RedirectToAction("index", "CurrencyAccounts");
        }








    }
}
