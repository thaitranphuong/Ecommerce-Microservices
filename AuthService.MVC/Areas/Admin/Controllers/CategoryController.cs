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
                TempData["result"] = JsonConvert.SerializeObject(new ToastifyModel()
                {
                    Status = "success",
                    Message = "Creating cartegory successfully!"
                });

                return Redirect("/admin/category");
            }
            else
            {
                TempData["result"] = JsonConvert.SerializeObject(new ToastifyModel()
                {
                    Status = "error",
                    Message = "Creating cartegory failed!"
                });

                return Redirect("/admin/category/add");
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_configuration["Host"]}/category/get/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var viewModel = JsonConvert.DeserializeObject<CategoryViewModel>(content);
                return View(viewModel);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryViewModel viewModel)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = new StringContent(
                JsonConvert.SerializeObject(viewModel),
                Encoding.UTF8,
                "application/json");

            var response = await client.PutAsync($"{_configuration["Host"]}/category/update", jsonData);

            if (response.IsSuccessStatusCode)
            {
                TempData["result"] = JsonConvert.SerializeObject(new ToastifyModel()
                {
                    Status = "success",
                    Message = "Editing cartegory successfully!"
                });

                return Redirect("/admin/category");
            }
            else
            {
                TempData["result"] = JsonConvert.SerializeObject(new ToastifyModel()
                {
                    Status = "error",
                    Message = "Editing cartegory failed!"
                });

                return Redirect("/admin/category");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"{_configuration["Host"]}/category/delete/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["result"] = JsonConvert.SerializeObject(new ToastifyModel()
                {
                    Status = "success",
                    Message = "Deleting cartegory successfully!"
                });

                return Redirect("/admin/category");
            }
            else
            {
                TempData["result"] = JsonConvert.SerializeObject(new ToastifyModel()
                {
                    Status = "error",
                    Message = "Deleting cartegory failed!"
                });

                return Redirect("/admin/category");
            }

        }
    }
}
