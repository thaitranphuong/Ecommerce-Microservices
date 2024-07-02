using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.MVC.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult ListProduct()
        {
            return View();
        }

        public IActionResult ProductDetail()
        {
            return View();
        }
    }
}
