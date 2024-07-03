using AuthService.MVC.Models;
using AuthService.MVC.Models.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ProductController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(string name = "", int page = 1, int limit = 4)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_configuration["Host"]}/product/get-all?name={name}&page={page}&limit={limit}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var viewModel = JsonConvert.DeserializeObject<ProductOutput>(content);
                return View(viewModel);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_configuration["Host"]}/category/get-all?name=&page=1&limit=100");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                CategoryOutput viewModel = JsonConvert.DeserializeObject<CategoryOutput>(content);
                return View(viewModel.ListResult);
            }
            else
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] ProductViewModel viewModel, [FromForm] IFormFile image)
        {
            var client = _httpClientFactory.CreateClient();
            var formData = new MultipartFormDataContent();

            var jsonContent = JsonContent.Create(viewModel);
            formData.Add(jsonContent, "product");

            if (image != null)
            {
                var fileContent = new StreamContent(image.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType);
                formData.Add(fileContent, "image", image.FileName);
            }

            var response = await client.PostAsync($"{_configuration["Host"]}/product/create", formData);

            if (response.IsSuccessStatusCode)
            {
                TempData["result"] = JsonConvert.SerializeObject(new ToastifyModel()
                {
                    Status = "success",
                    Message = "Creating product successfully!"
                });

                return Redirect("/admin/product");
            }
            else
            {
                TempData["result"] = JsonConvert.SerializeObject(new ToastifyModel()
                {
                    Status = "error",
                    Message = "Creating product failed!"
                });

                return Redirect("/admin/product/add");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ShowHide([FromRoute] int id, [FromForm] bool enabled)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_configuration["Host"]}/product/get/{id}");
            var message = enabled ? "Show" : "Hide";

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                ProductViewModel viewModel = JsonConvert.DeserializeObject<ProductViewModel>(content);
                viewModel.Enabled = enabled;

                var jsonData = new StringContent(
                JsonConvert.SerializeObject(viewModel),
                Encoding.UTF8,
                "application/json");

                response = await client.PutAsync($"{_configuration["Host"]}/product/update", jsonData);

                if (response.IsSuccessStatusCode)
                {
                    TempData["result"] = JsonConvert.SerializeObject(new ToastifyModel()
                    {
                        Status = "success",
                        Message = $"{message} product successfully!"
                    });

                    return Redirect("/admin/product");
                }
                else
                {
                    TempData["result"] = JsonConvert.SerializeObject(new ToastifyModel()
                    {
                        Status = "error",
                        Message = $"{message} product failed!"
                    });

                    return Redirect("/admin/product");
                }
            }
            else
            {
                TempData["result"] = JsonConvert.SerializeObject(new ToastifyModel()
                {
                    Status = "error",
                    Message = $"{message} product failed!"
                });

                return Redirect("/admin/product");
            }
        }

        public IActionResult Edit()
        {
            return View();
        }
    }
}
