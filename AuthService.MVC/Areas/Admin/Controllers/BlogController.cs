using AuthService.MVC.Constants;
using AuthService.MVC.Helpers;
using AuthService.MVC.Models;
using AuthService.MVC.Models.Pagination;
using AuthService.MVC.SyncServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AuthService.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BlogController : Controller
    {
        private readonly IApiService _apiService;

        public BlogController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index(string title = "", int page = 1, int limit = 4)
        {
            var response = await _apiService.GetAsync($"/blog/get-all?title={title}&page={page}&limit={limit}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var viewModel = JsonConvert.DeserializeObject<BlogOutput>(content);
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
        public async Task<IActionResult> Add([FromForm] BlogViewModel viewModel, [FromForm] IFormFile thumbnail)
        {
            var formData = new MultipartFormDataContent();

            var jsonContent = JsonContent.Create(viewModel);
            formData.Add(jsonContent, "blog");

            if (thumbnail != null)
            {
                var fileContent = new StreamContent(thumbnail.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(thumbnail.ContentType);
                formData.Add(fileContent, "thumbnail", thumbnail.FileName);
            }

            var response = await _apiService.PostFormDataAsync($"/blog/create", formData);

            if (response.IsSuccessStatusCode)
            {
                TempData["result"] = TempDataGenerator.Generate(NotificationType.Success, "Creating blog successfully!");
                return Redirect("/admin/blog");
            }
            else
            {
                TempData["result"] = TempDataGenerator.Generate(NotificationType.Error, "Creating blog failed!");
                return Redirect("/admin/blog/add");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] string id)
        {
            var response = await _apiService.GetAsync($"/blog/get/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var viewModel = JsonConvert.DeserializeObject<BlogViewModel>(content);
                return View(viewModel);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BlogViewModel viewModel, [FromForm] IFormFile thumbnail)
        {
            var formData = new MultipartFormDataContent();

            var jsonContent = JsonContent.Create(viewModel);
            formData.Add(jsonContent, "blog");

            if (thumbnail != null)
            {
                var fileContent = new StreamContent(thumbnail.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(thumbnail.ContentType);
                formData.Add(fileContent, "thumbnail", thumbnail.FileName);
            }

            var response = await _apiService.PutFormDataAsync($"/blog/update", formData);

            if (response.IsSuccessStatusCode)
            {
                TempData["result"] = TempDataGenerator.Generate(NotificationType.Success, "Editing blog successfullly!");
                return Redirect("/admin/blog");
            }
            else
            {
                TempData["result"] = TempDataGenerator.Generate(NotificationType.Error, "Editing blog failed!");
                return Redirect("/admin/blog");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var response = await _apiService.DeleteAsync($"/blog/delete/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["result"] = TempDataGenerator.Generate(NotificationType.Success, "Deleting blog successfully!");
                return Redirect("/admin/blog");
            }
            else
            {
                TempData["result"] = TempDataGenerator.Generate(NotificationType.Error, "Deleting blog failed!");
                return Redirect("/admin/blog");
            }

        }
    }
}
