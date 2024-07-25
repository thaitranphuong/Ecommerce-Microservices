using AuthService.MVC.Models;
using AuthService.MVC.SyncServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IApiService _apiService;

        public HomeController(AppDbContext context, IApiService apiService)
        {
            _context = context;
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["userCount"] = _context.Users.ToList().Count;
            ViewData["messageCount"] = _context.Messages.Where(m => m.CreatedTime.Day == DateTime.Now.Day).ToList().Count;
            var response = await _apiService.GetAsync($"/order/get-all-by-year?year={DateTime.Now.Year}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var orders = JsonConvert.DeserializeObject<List<OrderViewModel>>(content);
                ViewData["orders"] = orders;
                float total = 0f;
                foreach (var order in orders)
                {
                    foreach (var orderDetail in order.OrderDetails)
                    {
                        total += orderDetail.Price * orderDetail.Quantity * (100 - order.VoucherDiscountPercent) / 100;
                    }
                }
                ViewData["total"] = total;

                var monthlyRevenue = new List<float>();
                for (int i=0; i<12; i++)
                {
                    var monthlyTotal = 0f;
                    foreach (var order in orders)
                    {
                        if(order.CreatedTime.Month == i + 1)
                            foreach (var orderDetail in order.OrderDetails)
                            {
                                monthlyTotal += orderDetail.Price * orderDetail.Quantity * (100 - order.VoucherDiscountPercent) / 100;
                            }
                    }
                    monthlyRevenue.Add(monthlyTotal);
                }
                ViewData["monthlyRevenue"] = monthlyRevenue;
            }
            

            return View();
        }
    }
}
