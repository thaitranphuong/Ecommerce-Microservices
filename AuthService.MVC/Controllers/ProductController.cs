using AuthService.MVC.Models;
using AuthService.MVC.Models.Pagination;
using AuthService.MVC.SyncServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.MVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IApiService _apiService;

        public ProductController(IApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public async Task<IActionResult> ListProduct(string name = "", int categoryId = 0, float price = 0, int page = 1, int limit = 6)
        {
            var response = await _apiService.GetAsync($"/category/get-all?name=&page=1&limit=100");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var categoriesViewModel = JsonConvert.DeserializeObject<CategoryOutput>(content);
                ViewData["categories"] = categoriesViewModel;
            }
            else
            {
                ViewData["categories"] = new CategoryOutput();
            }

            response = await _apiService.GetAsync($"/product/get-all-customer?name={name}&categoryId={categoryId}&price={price}&page={page}&limit={limit}");
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
        public async Task<IActionResult> ProductDetail([FromRoute] int id)
        {
            var response = await _apiService.GetAsync($"/product/get/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var viewModel = JsonConvert.DeserializeObject<ProductViewModel>(content);

                response = await _apiService.GetAsync($"/product/get-all-customer?name=&categoryId={viewModel.CategoryId}&price=0&page=1&limit=10");
                if (response.IsSuccessStatusCode)
                {
                    content = await response.Content.ReadAsStringAsync();
                    var relativeProducts = JsonConvert.DeserializeObject<ProductOutput>(content);
                    ViewData["relativeProducts"] = relativeProducts;
                }
                else
                {
                    ViewData["relativeProducts"] = new ProductOutput();
                }

                response = await _apiService.GetAsync($"/comment/get-all/{id}?&page=1&limit=100");
                if (response.IsSuccessStatusCode)
                {
                    content = await response.Content.ReadAsStringAsync();
                    var relativeProducts = JsonConvert.DeserializeObject<CommentOutput>(content);
                    ViewData["comments"] = relativeProducts;
                }
                else
                {
                    ViewData["comments"] = new CommentOutput();
                }

                return View(viewModel);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Comment(CommentViewModel comment)
        {
            var response = await _apiService.PostAsync("/comment/create", comment);
            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }
            else
            {
                return NoContent();
            }
        }

        [HttpGet]
        [Route("/Product/GetComments/{productId}")]
        public async Task<IActionResult> GetComments(int productId)
        {
            var response = await _apiService.GetAsync($"/comment/get-all/{productId}?&page=1&limit=100");
            if(response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var commentOutput = JsonConvert.DeserializeObject<CommentOutput>(content);
                return PartialView("Components/_CommentList", commentOutput);
            }
            else
            {
                return NoContent();
            }
        }
    }
}
