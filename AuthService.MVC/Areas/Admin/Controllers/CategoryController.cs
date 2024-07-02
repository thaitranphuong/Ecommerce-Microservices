using AuthService.MVC.Models;
using AuthService.MVC.Models.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public CategoryController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(string name = "", int page = 1, int limit = 4)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_configuration["Host"]}/category/get-all?name={name}&page={page}&limit={limit}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var viewModel = JsonConvert.DeserializeObject<CategoryOutput>(content);
                return View(viewModel);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(CategoryViewModel viewModel)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = new StringContent(
                JsonConvert.SerializeObject(viewModel),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync($"{_configuration["Host"]}/category/create", jsonData);

            if (response.IsSuccessStatusCode)
            {
                TempData["Result"] = "Create success!";
                
                return RedirectToRoute("/Admin/Category/Index");
            }
            else
            {
                TempData["Result"] = "Error!";
                return View("Add");
            }
            
        }

        public IActionResult Edit()
        {
            return View();
        }
    }
}
