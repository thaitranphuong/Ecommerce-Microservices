using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AuthService.MVC.Areas.Admin.Pages.Role
{
    //[Authorize(Roles = "Admin")]
    public class IndexModel : RolePageModel
    {
        public IndexModel(RoleManager<IdentityRole> roleManager, AppDbContext context) : base(roleManager, context)
        {
        }

        public List<IdentityRole> roles { set; get; }

        public async Task OnGet()
        {
            roles = await _roleManager.Roles.OrderBy(r => r.Name).ToListAsync();
        }

        public void OnPost() => RedirectToPage();
    }
}
