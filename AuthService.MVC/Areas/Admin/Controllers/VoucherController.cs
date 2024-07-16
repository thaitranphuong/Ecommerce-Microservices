using AuthService.MVC.Constants;
using AuthService.MVC.Helpers;
using AuthService.MVC.Models;
using AuthService.MVC.Models.Pagination;
using AuthService.MVC.SyncServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace AuthService.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class VoucherController : Controller
    {
        private readonly IApiService _apiService;

        public VoucherController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index(string name = "", int page = 1, int limit = 4)
        {
            var response = await _apiService.GetAsync($"/voucher/get-all?name={name}&page={page}&limit={limit}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var viewModel = JsonConvert.DeserializeObject<VoucherOutput>(content);
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
        public async Task<IActionResult> Add(VoucherViewModel viewModel)
        {
            var response = await _apiService.PostAsync("/voucher/create", viewModel);

            if (response.IsSuccessStatusCode)
            {
                TempData["result"] = TempDataGenerator.Generate(NotificationType.Success, "Creating voucher successfully!");
                return Redirect("/admin/voucher");
            }
            else
            {
                TempData["result"] = TempDataGenerator.Generate(NotificationType.Error, "Creating voucher failed!");
                return Redirect("/admin/voucher/add");
            }

        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var response = await _apiService.GetAsync($"/voucher/get/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var viewModel = JsonConvert.DeserializeObject<VoucherViewModel>(content);
                return View(viewModel);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(VoucherViewModel viewModel)
        {
            var response = await _apiService.PutAsync("/voucher/update", viewModel);

            if (response.IsSuccessStatusCode)
            {
                TempData["result"] = TempDataGenerator.Generate(NotificationType.Success, "Editing voucher successfully!");
                return Redirect("/admin/voucher");
            }
            else
            {
                TempData["result"] = TempDataGenerator.Generate(NotificationType.Error, "Creating voucher failed!");
                return Redirect("/admin/voucher");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = await _apiService.DeleteAsync($"/voucher/delete/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["result"] = TempDataGenerator.Generate(NotificationType.Success, "Deleting voucher successfully!");
                return Redirect("/admin/voucher");
            }
            else
            {
                TempData["result"] = TempDataGenerator.Generate(NotificationType.Error, "Deleting voucher failed!");
                return Redirect("/admin/voucher");
            }

        }
    }
}
