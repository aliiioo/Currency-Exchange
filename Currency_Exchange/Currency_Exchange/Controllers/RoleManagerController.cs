using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Data;

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

        public async Task<bool> AssignUserToRole(string username, string role)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (!user.EmailConfirmed) return false;
            if (!await _roleManager.RoleExistsAsync(role) != null)
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
            await _userManager.AddToRoleAsync(user, "Admin");
            return true;

        }
    }
}
