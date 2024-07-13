using Microsoft.AspNetCore.Mvc;

namespace Currency_Exchange.Controllers
{
    public class CurrencyAccountsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
