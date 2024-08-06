using System.Transactions;
using Application.Contracts.Persistence;
using Application.Dtos.RegistrationDto;
using Currency_Exchange.Security;
using Domain.Entities;
using Infrastructure.Repositories.Persistence;
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

        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string? error)
        {
            var referrerUrl = HttpContext.Request.Headers["Referer"].ToString();
            ViewBag.error = error;
            if (!string.IsNullOrEmpty(referrerUrl))
            {
                ViewBag.returnUrl = referrerUrl;
            }
            return View();
        }
    }
}