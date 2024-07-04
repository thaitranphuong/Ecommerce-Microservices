using AuthService.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.MVC.Controllers
{
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
            var role = await _roleManager.FindByNameAsync("Admin");
            if (role == null)
            {
                return View(new List<AppUser>());
            }

            var usersInRole = await _userManager.GetUsersInRoleAsync("Admin");
            return View(usersInRole.ToList());
        }

        [HttpPost]
        [Route("/api/get-list-message")]
        public async Task<IActionResult> getListMessage(string senderId, string receiverId)
        {
            var listMessage = await _context.Messages
                .Where(m => m.SenderId == senderId && m.ReceiverId == receiverId 
                            || m.SenderId == receiverId && m.ReceiverId == senderId)
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .ToListAsync();

            return Ok(new { listMessage = listMessage });
        }
    }
}
