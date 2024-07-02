using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.MVC.Controllers
{
    public class VoucherController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
