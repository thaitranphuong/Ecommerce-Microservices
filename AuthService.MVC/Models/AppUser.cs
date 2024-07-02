using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.MVC.Models
{
    public class AppUser : IdentityUser
    {
        public string Avatar { get; set; }
    }
}
