using System.Reflection.PortableExecutable;
using Application.Contracts.Persistence;
using Application.Dtos.CurrencyDtos;
using Microsoft.AspNetCore.Mvc;

namespace Currency_Exchange.Controllers
{
    public class FeeExchangeController : Controller
    {
        private readonly ICurrencyServices _currencyServices;

        public FeeExchangeController(ICurrencyServices currencyServices)
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
            var fee = await _currencyServices.CreateExchangeFeeToCurrency(Model);
            if (fee == 0)
            {
                return View(Model);
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Update(int exchangeFeeId)
        {
            var exchangeFee = await _currencyServices.GetExchangeFeeCurrencyByIdAsync(exchangeFeeId);
            return View(exchangeFee);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateFeeDtos feeDto)
        {
            if (!ModelState.IsValid)
            {
                return View(feeDto);
            }
            var transformFee = await _currencyServices.UpdateExchangeFeeToCurrency(feeDto);
            return RedirectToAction("index", "CurrencyAccounts");
        }





    }
}
