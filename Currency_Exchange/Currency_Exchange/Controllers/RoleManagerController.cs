using Application.Contracts.Persistence;
using Application.Dtos.RegistrationDto;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Currency_Exchange.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleManagerController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAdminServices _adminServices;

        public RoleManagerController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IAdminServices adminServices)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _adminServices = adminServices;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var adminUsers = await _userManager.GetUsersInRoleAsync("Admin");
            var listUserDto = new List<UserDto>();
            foreach (var item in adminUsers)
            {
                var userDto= new UserDto
                {
                    Email = item.Email,
                    FullName = item.FullName,
                    PhoneNumber = item.PhoneNumber,
                    IsAdmin = true
                };
                listUserDto.Add(userDto);
            }

            return View(listUserDto);
        }

        [HttpGet]

        public async Task<IActionResult> IndexCustomer()
        {
            var users = await _userManager.GetUsersInRoleAsync("Customer");
            var listUserDto = new List<UserDto>();
            foreach (var item in users)
            {
                var userDto = new UserDto();
                userDto.Email = item.Email;
                userDto.FullName = item.FullName;
                userDto.PhoneNumber = item.PhoneNumber;
                userDto.IsAdmin = false;
                listUserDto.Add(userDto);
            }

            return View(listUserDto);
        }

        public async Task<IActionResult> AssignUserToRole(string email, string role="Admin")
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (!user.EmailConfirmed) return RedirectToAction("Index");

            if (await _roleManager.RoleExistsAsync(role) == false)
            {
                return RedirectToAction("Index");
            }
            await _userManager.AddToRoleAsync(user, role);
            await _userManager.RemoveFromRoleAsync(user, "Customer");
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> RemoveUserRole(string email)
        {
            var user = await _userManager.FindByNameAsync(email);
            if (!user.EmailConfirmed) return RedirectToAction("Index");
            await _userManager.RemoveFromRoleAsync(user, "Admin");
            await _userManager.AddToRoleAsync(user, "Customer");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UsersAccounts(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest();
            }
            var accounts =await _adminServices.GetUsersAccountsForAdmin(email);
            return View(accounts);
        }

    }
}
