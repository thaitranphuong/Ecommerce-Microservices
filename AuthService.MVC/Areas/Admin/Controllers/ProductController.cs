using AuthService.MVC.Constants;
using AuthService.MVC.Helpers;
using AuthService.MVC.Models;
using AuthService.MVC.Models.Pagination;
using AuthService.MVC.SyncServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AuthService.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IApiService _apiService;

        public ProductController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index(string name = "", int page = 1, int limit = 4)
        {
            var response = await _apiService.GetAsync($"/product/get-all?name={name}&page={page}&limit={limit}");
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
            var response = await _apiService.GetAsync($"/category/get-all?name=&page=1&limit=100");
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
            var formData = new MultipartFormDataContent();

            var jsonContent = JsonContent.Create(viewModel);
            formData.Add(jsonContent, "product");

            if (image != null)
            {
                var fileContent = new StreamContent(image.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType);
                formData.Add(fileContent, "image", image.FileName);
            }

            var response = await _apiService.PostFormDataAsync($"/product/create", formData);

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

                await _apiService.PostFormDataAsync($"/productdetail/create", formData);

                TempData["result"] = TempDataGenerator.Generate(NotificationType.Success, "Creating product successfully!");
                return Redirect("/admin/product");
            }
            else
            {
                TempData["result"] = TempDataGenerator.Generate(NotificationType.Error, "Creating product failed!");
                return Redirect("/admin/product/add");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ShowHide([FromRoute] int id, [FromForm] bool enabled)
        {
            var message = enabled ? "Show" : "Hide";
            var response = await _apiService.GetAsync($"/product/get/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                ProductViewModel viewModel = JsonConvert.DeserializeObject<ProductViewModel>(content);
                viewModel.Enabled = enabled;

                var formData = new MultipartFormDataContent();

                var jsonContent = JsonContent.Create(viewModel);
                formData.Add(jsonContent, "product");

                response = await _apiService.PutFormDataAsync($"/product/showhide", formData);

                if (response.IsSuccessStatusCode)
                {
                    TempData["result"] = TempDataGenerator.Generate(NotificationType.Success, $"{message} product successfully!");
                    return Redirect("/admin/product");
                }
                else
                {
                    TempData["result"] = TempDataGenerator.Generate(NotificationType.Error, $"{message} product failed!");
                    return Redirect("/admin/product");
                }
            }
            else
            {
                TempData["result"] = TempDataGenerator.Generate(NotificationType.Error, $"{message} product failed!");
                return Redirect("/admin/product");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var response = await _apiService.GetAsync($"/category/get-all?name=&page=1&limit=100");
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

            response = await _apiService.GetAsync($"/product/get/{id}");
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
            var formData = new MultipartFormDataContent();

            var jsonContent = JsonContent.Create(viewModel);
            formData.Add(jsonContent, "product");

            if (image != null)
            {
                var fileContent = new StreamContent(image.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType);
                formData.Add(fileContent, "image", image.FileName);
            }

            var response1 = await _apiService.PutFormDataAsync($"/product/update", formData);

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

            var response2 = await _apiService.PostFormDataAsync($"/productdetail/create", formData);

            var response3 = await _apiService.PostAsync($"/productdetail/delete", removedProductDetailIds);

            if (response1.IsSuccessStatusCode || response2.IsSuccessStatusCode || response3.IsSuccessStatusCode)
            {
                TempData["result"] = TempDataGenerator.Generate(NotificationType.Success, "Editing product successfullly!");
                return Redirect("/admin/product");
            }
            else
            {
                TempData["result"] = TempDataGenerator.Generate(NotificationType.Error, "Editing product failed!");
                return Redirect("/admin/product");
            }
        }
    }
}
