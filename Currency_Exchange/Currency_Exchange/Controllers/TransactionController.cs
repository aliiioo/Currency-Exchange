using Microsoft.AspNetCore.Mvc;

namespace Currency_Exchange.Controllers
{
    public class TransactionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
