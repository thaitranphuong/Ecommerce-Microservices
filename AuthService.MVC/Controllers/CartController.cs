using AuthService.MVC.Constants;
using AuthService.MVC.Helpers;
using AuthService.MVC.Models;
using AuthService.MVC.Models.Pagination;
using AuthService.MVC.SyncServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthService.MVC.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IApiService _apiService;
        private readonly UserManager<AppUser> _userManager;

        public CartController(IApiService apiService, UserManager<AppUser> userManager)
        {
            _apiService = apiService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var voucher = new VoucherOutput();
            var response = await _apiService.GetAsync($"/voucher/get-all-of-customer-page?name=&page=1&limit=100");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                voucher = JsonConvert.DeserializeObject<VoucherOutput>(content);
            }
            else
            {
                voucher.ListResult = new List<VoucherViewModel>();
            }
            ViewData["VoucherList"] = voucher.ListResult;

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found");
            }
            response = await _apiService.GetAsync($"/cart/get-all/{user.Id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var viewModel = JsonConvert.DeserializeObject<List<CartItemViewModel>>(content);
                return View(viewModel);
            }
            else
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCartItem()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var response = await _apiService.GetAsync($"/cart/get-all/{user.Id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var viewModel = JsonConvert.DeserializeObject<List<CartItemViewModel>>(content);
                return PartialView("Components/_CartItemList", viewModel);
            }
            else
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCountCartItem()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var response = await _apiService.GetAsync($"/cart/get-all/{user.Id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var viewModel = JsonConvert.DeserializeObject<List<CartItemViewModel>>(content);
                return Ok(viewModel.Count);
            }
            else
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(CartItemViewModel cartItem)
        {
            var response = await _apiService.PostAsync("/cart/create", cartItem);
            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }
            else
            {
                return StatusCode(500);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(CartItemViewModel cartItem)
        {
            var response = await _apiService.PutAsync("/cart/update", cartItem);
            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }
            else
            {
                return StatusCode(500);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string userId, [FromQuery] int productId)
        {
            var response = await _apiService.DeleteAsync($"/cart/delete/{userId}/{productId}");
            if (response.IsSuccessStatusCode)
            {
                return Redirect("/Cart/Index");
            }
            else
            {
                return StatusCode(500);
            }
        }
    }
}
