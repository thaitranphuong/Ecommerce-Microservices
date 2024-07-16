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
    public class VoucherController : Controller
    {
        private readonly IApiService _apiService;

        public VoucherController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index(string name = "", int page = 1, int limit = 4)
        {
            var response = await _apiService.GetAsync($"/voucher/get-all-of-customer-page?name={name}&page={page}&limit={limit}");
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
    }
}
