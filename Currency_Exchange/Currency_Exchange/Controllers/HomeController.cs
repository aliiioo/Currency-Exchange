using Application.Dtos.RegistrationDto;
using Currency_Exchange.Security;
using Microsoft.AspNetCore.Mvc;

namespace Currency_Exchange.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {

        }
        [ServiceFilter(typeof(SanitizeInputFilter))]
        public IActionResult Index()
        {
            return View();

            var userDto = new UserDto()
            {
                IsAdmin = false,
                Email = "Abbas@abbas.com",
                FullName = "DELETE FROM dbo.Transactions;"
            };

            var sanitizeInputFilter = new SanitizeInputFilter();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string? error)
        {
            ViewBag.error = error;
            return View();
        }
    }
}