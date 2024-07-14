using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using Application.Dtos.RegistrationDto;

namespace Currency_Exchange.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleManagerController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
       
        public RoleManagerController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.GetUsersInRoleAsync("Admin");
            var listUserDto = new List<UserDto>();
            foreach (var item in users)
            {
                var userDto= new UserDto();
                userDto.Email=item.Email;
                userDto.FullName=item.FullName;
                userDto.PhoneNumber=item.PhoneNumber;
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
                listUserDto.Add(userDto);
            }

            return View(listUserDto);
        }

        public async Task<bool> AssignUserToRole(string username, string role="Admin")
        {
            var user = await _userManager.FindByNameAsync(username);
            if (!user.EmailConfirmed) return false;

            if (!await _roleManager.RoleExistsAsync(role) == false)
            {
                return false;
            }
            await _userManager.AddToRoleAsync(user, role);
            return true;

        }

        public async Task<bool> RemoveUserRole(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (!user.EmailConfirmed) return false;
            await _userManager.RemoveFromRoleAsync(user, "Admin");
            await _userManager.AddToRoleAsync(user, "Customer");
            return true;

        }
    }
}
