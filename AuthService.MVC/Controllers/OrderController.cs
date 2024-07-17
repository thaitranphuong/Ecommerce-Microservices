using AuthService.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.MVC.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Checkout([FromForm] CheckoutViewModel checkoutViewModel)
        {
            var orderViewModel = checkoutViewModel.Order;
            var orderDetailsViewModel = checkoutViewModel.OrderDetails;
            orderViewModel.OrderDetails = orderDetailsViewModel;
            
            return View();
        }
    }
}
