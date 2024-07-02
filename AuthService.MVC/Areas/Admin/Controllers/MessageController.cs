using AuthService.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class MessageController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;

        public MessageController(UserManager<AppUser> userManager, AppDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var role = await _roleManager.FindByNameAsync("Customer");
            if (role == null)
            {
                return View(new List<AppUser>());
            }

            var usersInRole = await _userManager.GetUsersInRoleAsync("Customer");
            return View(usersInRole.ToList());
        }
    }
}
