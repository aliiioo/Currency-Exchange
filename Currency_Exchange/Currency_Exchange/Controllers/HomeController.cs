using Application.API_Calls;
using Currency_Exchange.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Currency_Exchange.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var httpClient = new HttpClient();
            var currencyService = new CurrencyService(httpClient);

            string baseCurrency = "EUR"; // Euro
            string targetCurrency = "USD"; // US Dollar

            try
            {
                decimal exchangeRate = await currencyService.GetExchangeRateAsync(baseCurrency, targetCurrency);
               
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}