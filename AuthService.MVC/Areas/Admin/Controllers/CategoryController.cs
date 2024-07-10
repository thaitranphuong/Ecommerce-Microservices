using AuthService.MVC.Constants;
using AuthService.MVC.Helpers;
using AuthService.MVC.Models;
using AuthService.MVC.Models.Pagination;
using AuthService.MVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace AuthService.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly IApiService _apiService;

        public CategoryController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index(string name = "", int page = 1, int limit = 4)
        {
            var response = await _apiService.GetAsync($"/category/get-all?name={name}&page={page}&limit={limit}");
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
            var response = await _apiService.PostAsync("/category/create", viewModel);

            if (response.IsSuccessStatusCode)
            {
                TempData["result"] = TempDataGenerator.Generate(NotificationType.Success, "Creating cartegory successfully!");
                return Redirect("/admin/category");
            }
            else
            {
                TempData["result"] = TempDataGenerator.Generate(NotificationType.Error, "Creating cartegory failed!");
                return Redirect("/admin/category/add");
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var response = await _apiService.GetAsync($"/category/get/{id}");
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
            var response = await _apiService.PutAsync("/category/update", viewModel);

            if (response.IsSuccessStatusCode)
            {
                TempData["result"] = TempDataGenerator.Generate(NotificationType.Success, "Editing cartegory successfully!");
                return Redirect("/admin/category");
            }
            else
            {
                TempData["result"] = TempDataGenerator.Generate(NotificationType.Error, "Creating cartegory failed!");
                return Redirect("/admin/category");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = await _apiService.DeleteAsync($"/category/delete/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["result"] = TempDataGenerator.Generate(NotificationType.Success, "Deleting cartegory successfully!");
                return Redirect("/admin/category");
            }
            else
            {
                TempData["result"] = TempDataGenerator.Generate(NotificationType.Error, "Deleting cartegory failed!");
                return Redirect("/admin/category");
            }

        }
    }
}
