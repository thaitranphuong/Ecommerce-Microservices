using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.MVC.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
