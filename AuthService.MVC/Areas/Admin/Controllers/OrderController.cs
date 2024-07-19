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

namespace AuthService.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IApiService _apiService;

        public OrderController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index(OrderStatus status = OrderStatus.ALL, int page = 1, int limit = 4)
        {
            var response = await _apiService.GetAsync($"/order/get-all?status={status}&page={page}&limit={limit}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var viewModel = JsonConvert.DeserializeObject<OrderOutput>(content);
                return View(viewModel);
            }
            else
            {
                return StatusCode(500);
            }
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

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var response = await _apiService.GetAsync($"/order/get/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var viewModel = JsonConvert.DeserializeObject<OrderViewModel>(content);
                return View(viewModel);
            }
            else
            {
                return StatusCode(500);
            }
        }
    }
}
