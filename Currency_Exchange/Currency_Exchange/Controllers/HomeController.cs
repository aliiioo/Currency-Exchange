using Application.API_Calls;
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
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string? error)
        {
            ViewBag.error=error;
            return View();
        }
    }
}