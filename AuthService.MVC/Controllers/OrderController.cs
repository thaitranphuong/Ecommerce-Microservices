using AuthService.MVC.Constants;
using AuthService.MVC.Models;
using AuthService.MVC.Models.Pagination;
using AuthService.MVC.SyncServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.MVC.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IApiService _apiService;
        private readonly UserManager<AppUser> _userManager;

        public OrderController(IApiService apiService, UserManager<AppUser> userManager)
        {
            _apiService = apiService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(OrderStatus status = OrderStatus.ALL,
                                                [FromQuery] bool isCheckout = false)
        {
            var user = await _userManager.GetUserAsync(User);
            var response = await _apiService.GetAsync($"/order/get-all-of-customer-page?status={status}&userId={user.Id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var viewModel = JsonConvert.DeserializeObject<List<OrderViewModel>>(content);
                if (isCheckout) ViewBag.IsCheckout = isCheckout;
                return View(viewModel);
            }
            else
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Checkout([FromForm] CheckoutViewModel checkoutViewModel)
        {
            var orderViewModel = checkoutViewModel.Order;
            var orderDetailsViewModel = checkoutViewModel.OrderDetails;
            orderViewModel.OrderDetails = orderDetailsViewModel;
            var response = await _apiService.PostAsync($"/order/create", orderViewModel);
            if (response.IsSuccessStatusCode)
            {
                return Redirect("/Order/Index?isCheckout=true");
            }
            ViewBag.IsNoCheckout = true;
            return View();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] OrderViewModel orderViewModel)
        {
            var response = await _apiService.PutAsync($"/order/update", orderViewModel);
            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }
            return StatusCode(500);
        }
    }
}
