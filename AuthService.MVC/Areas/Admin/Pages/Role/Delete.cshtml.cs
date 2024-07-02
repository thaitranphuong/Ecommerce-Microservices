using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.MVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthService.MVC.Areas.Admin.Pages.Role
{
    public class DeleteModel : RolePageModel
    {
        public DeleteModel(RoleManager<IdentityRole> roleManager, AppDbContext context) : base(roleManager, context)
        {

        }


        public IdentityRole role { get; set; }

        public async Task<IActionResult> OnGet(string roleid)
        {
            if (roleid == null) return NotFound("Không tìm thấy role");

            role = await _roleManager.FindByIdAsync(roleid);
            if (role == null)
            {
                return NotFound("Không tìm thấy role");
            }
            return Page();
        }



        public async Task<IActionResult> OnPostAsync(string roleid)
        {
            if (roleid == null) return NotFound("Không tìm thấy role");
            role = await _roleManager.FindByIdAsync(roleid);
            if (role == null) return NotFound("Không tìm thấy role");


            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                StatusMessage = $"Bạn vừa xóa: {role.Name}";
                return RedirectToPage("./Index");
            }
            else
            {
                result.Errors.ToList().ForEach(error => {
                    ModelState.AddModelError(string.Empty, error.Description);
                });
            }
            return Page();
        }
    }
}
