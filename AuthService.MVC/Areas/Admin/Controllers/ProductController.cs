using AuthService.MVC.Models;
using AuthService.MVC.Models.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        public async Task<IActionResult> Add([FromForm] ProductViewModel viewModel, [FromForm] IFormFile image,
                                                [FromForm] IFormFile[] productDetails)
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
                var content = await response.Content.ReadAsStringAsync();
                int productId = JsonConvert.DeserializeObject<int>(content);
                var productDetailViewModels = new List<ProductDetailViewModel>();

                for (int i = 0; i < productDetails.Length; i++)
                {
                    var productDetailViewModel = new ProductDetailViewModel() { ProductId = productId };
                    productDetailViewModels.Add(productDetailViewModel);
                }

                formData = new MultipartFormDataContent();
                jsonContent = JsonContent.Create(productDetailViewModels);
                formData.Add(jsonContent, "productDetails");

                foreach (var productDetail in productDetails)
                {
                    var fileContent = new StreamContent(productDetail.OpenReadStream());
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(productDetail.ContentType);
                    formData.Add(fileContent, "productDetailImages", productDetail.FileName);
                }

                await client.PostAsync($"{_configuration["Host"]}/productdetail/create", formData);

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
            var message = enabled ? "Show" : "Hide";
            var response = await client.GetAsync($"{_configuration["Host"]}/product/get/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                ProductViewModel viewModel = JsonConvert.DeserializeObject<ProductViewModel>(content);
                viewModel.Enabled = enabled;

                var formData = new MultipartFormDataContent();

                var jsonContent = JsonContent.Create(viewModel);
                formData.Add(jsonContent, "product");

                response = await client.PutAsync($"{_configuration["Host"]}/product/showhide", formData);

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

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync($"{_configuration["Host"]}/category/get-all?name=&page=1&limit=100");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<CategoryOutput>(content).ListResult;
                ViewData["categories"] = categories;
            }
            else
            {
                return StatusCode(500);
            }

            response = await client.GetAsync($"{_configuration["Host"]}/product/get/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var viewModel = JsonConvert.DeserializeObject<ProductViewModel>(content);
                return View(viewModel);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductViewModel viewModel, [FromForm] IFormFile image,
                                                [FromForm] IFormFile[] newProductDetails,
                                                [FromForm] int[] removedProductDetailIds)
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

            var response1 = await client.PutAsync($"{_configuration["Host"]}/product/update", formData);

            var productDetailViewModels = new List<ProductDetailViewModel>();

            for (int i = 0; i < newProductDetails.Length; i++)
            {
                var productDetailViewModel = new ProductDetailViewModel() { ProductId = viewModel.Id };
                productDetailViewModels.Add(productDetailViewModel);
            }

            formData = new MultipartFormDataContent();
            jsonContent = JsonContent.Create(productDetailViewModels);
            formData.Add(jsonContent, "productDetails");

            foreach (var productDetail in newProductDetails)
            {
                var fileContent = new StreamContent(productDetail.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(productDetail.ContentType);
                formData.Add(fileContent, "productDetailImages", productDetail.FileName);
            }

            var response2 = await client.PostAsync($"{_configuration["Host"]}/productdetail/create", formData);

            var jsonData = new StringContent(
                JsonConvert.SerializeObject(removedProductDetailIds),
                Encoding.UTF8,
                "application/json");

            var response3 = await client.PostAsync($"{_configuration["Host"]}/productdetail/delete", jsonData);

            if (response1.IsSuccessStatusCode || response2.IsSuccessStatusCode || response3.IsSuccessStatusCode)
            {
                TempData["result"] = JsonConvert.SerializeObject(new ToastifyModel()
                {
                    Status = "success",
                    Message = "Editing product successfully!"
                });

                return Redirect("/admin/product");
            }
            else
            {
                TempData["result"] = JsonConvert.SerializeObject(new ToastifyModel()
                {
                    Status = "error",
                    Message = "Editing product failed!"
                });

                return Redirect("/admin/product");
            }
        }
    }
}
