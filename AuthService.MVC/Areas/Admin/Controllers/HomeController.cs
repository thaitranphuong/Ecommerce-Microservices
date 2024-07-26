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

                var monthlyOrders = new Dictionary<Constants.OrderStatus, int>();
                monthlyOrders.Add(Constants.OrderStatus.PENDING, 0);
                monthlyOrders.Add(Constants.OrderStatus.DELIVERING, 0);
                monthlyOrders.Add(Constants.OrderStatus.RECEIVED, 0);
                monthlyOrders.Add(Constants.OrderStatus.CANCELED, 0);
                int value;
                foreach (var order in orders)
                {
                    switch (order.Status)
                    {
                        case Constants.OrderStatus.PENDING:
                            if (monthlyOrders.TryGetValue(Constants.OrderStatus.PENDING, out value))
                                monthlyOrders[Constants.OrderStatus.PENDING] = value + 1;
                            else
                                monthlyOrders[Constants.OrderStatus.PENDING] = 1;
                            break;
                        case Constants.OrderStatus.DELIVERING:
                            if (monthlyOrders.TryGetValue(Constants.OrderStatus.DELIVERING, out value))
                                monthlyOrders[Constants.OrderStatus.DELIVERING] = value + 1;
                            else
                                monthlyOrders[Constants.OrderStatus.DELIVERING] = 1;
                            break;
                        case Constants.OrderStatus.RECEIVED:
                            if (monthlyOrders.TryGetValue(Constants.OrderStatus.RECEIVED, out value))
                                monthlyOrders[Constants.OrderStatus.RECEIVED] = value + 1;
                            else
                                monthlyOrders[Constants.OrderStatus.RECEIVED] = 1;
                            break;
                        case Constants.OrderStatus.CANCELED:
                            if (monthlyOrders.TryGetValue(Constants.OrderStatus.CANCELED, out value))
                                monthlyOrders[Constants.OrderStatus.CANCELED] = value + 1;
                            else
                                monthlyOrders[Constants.OrderStatus.CANCELED] = 1;
                            break;
                    }
                }
                ViewData["monthlyOrders"] = monthlyOrders;
            }
            return View();
        }

        [HttpGet]
        [Route("/Admin/GetMonthlyRevenue")]
        public async Task<IActionResult> GetMonthlyRevenue([FromQuery] int year)
        {
            var response = await _apiService.GetAsync($"/order/get-all-by-year?year={year}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var orders = JsonConvert.DeserializeObject<List<OrderViewModel>>(content);
                var monthlyRevenue = new List<float>();
                for (int i = 0; i < 12; i++)
                {
                    var monthlyTotal = 0f;
                    foreach (var order in orders)
                    {
                        if (order.CreatedTime.Month == i + 1)
                            foreach (var orderDetail in order.OrderDetails)
                            {
                                monthlyTotal += orderDetail.Price * orderDetail.Quantity * (100 - order.VoucherDiscountPercent) / 100;
                            }
                    }
                    monthlyRevenue.Add(monthlyTotal);
                }
                return Ok(monthlyRevenue);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("/Admin/GetMonthlyOrders")]
        public async Task<IActionResult> GetMonthlyOrder([FromQuery] int year, [FromQuery] int month)
        {
            var response = await _apiService.GetAsync($"/order/get-all-by-year?year={year}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var orders = JsonConvert.DeserializeObject<List<OrderViewModel>>(content);
                var monthlyOrders = new Dictionary<Constants.OrderStatus, int>();
                monthlyOrders.Add(Constants.OrderStatus.PENDING, 0);
                monthlyOrders.Add(Constants.OrderStatus.DELIVERING, 0);
                monthlyOrders.Add(Constants.OrderStatus.RECEIVED, 0);
                monthlyOrders.Add(Constants.OrderStatus.CANCELED, 0);
                int value;
                foreach (var order in orders)
                {
                    if(order.CreatedTime.Month == month)
                    {
                        switch (order.Status)
                        {
                            case Constants.OrderStatus.PENDING:
                                if (monthlyOrders.TryGetValue(Constants.OrderStatus.PENDING, out value))
                                    monthlyOrders[Constants.OrderStatus.PENDING] = value + 1;
                                else
                                    monthlyOrders[Constants.OrderStatus.PENDING] = 1;
                                break;
                            case Constants.OrderStatus.DELIVERING:
                                if (monthlyOrders.TryGetValue(Constants.OrderStatus.DELIVERING, out value))
                                    monthlyOrders[Constants.OrderStatus.DELIVERING] = value + 1;
                                else
                                    monthlyOrders[Constants.OrderStatus.DELIVERING] = 1;
                                break;
                            case Constants.OrderStatus.RECEIVED:
                                if (monthlyOrders.TryGetValue(Constants.OrderStatus.RECEIVED, out value))
                                    monthlyOrders[Constants.OrderStatus.RECEIVED] = value + 1;
                                else
                                    monthlyOrders[Constants.OrderStatus.RECEIVED] = 1;
                                break;
                            case Constants.OrderStatus.CANCELED:
                                if (monthlyOrders.TryGetValue(Constants.OrderStatus.CANCELED, out value))
                                    monthlyOrders[Constants.OrderStatus.CANCELED] = value + 1;
                                else
                                    monthlyOrders[Constants.OrderStatus.CANCELED] = 1;
                                break;
                        }
                    }
                }
                return Ok(monthlyOrders);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
