using AuthService.MVC.AsyncServices;
using AuthService.MVC.Constants;
using AuthService.MVC.Dtos;
using AuthService.MVC.Models;
using AuthService.MVC.Models.Pagination;
using AuthService.MVC.SyncServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IApiService _apiService;

        public HomeController(IApiService apiService)
        {
            _apiService = apiService;
        }


        public async Task<IActionResult> Index()
        {
            var response = await _apiService.GetAsync($"/product/get-all-customer?page=1&limit=8");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var viewModel = JsonConvert.DeserializeObject<ProductOutput>(content);
                return View(viewModel);
            }
            else
            {
                return View();
            }
        }
    }
}
