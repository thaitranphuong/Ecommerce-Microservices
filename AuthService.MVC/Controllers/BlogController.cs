using AuthService.MVC.Models;
using AuthService.MVC.Models.Pagination;
using AuthService.MVC.SyncServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthService.MVC.Controllers
{
    public class BlogController : Controller
    {
        private readonly IApiService _apiService;

        public BlogController(IApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string title = "", int page = 1, int limit = 6)
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
        public async Task<IActionResult> Detail([FromRoute] string id)
        {
            var response = await _apiService.GetAsync($"/blog/get/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var viewModel = JsonConvert.DeserializeObject<BlogViewModel>(content);

                response = await _apiService.GetAsync($"/blog/comment/get-all/{id}");
                if (response.IsSuccessStatusCode)
                {
                    content = await response.Content.ReadAsStringAsync();
                    var comments = JsonConvert.DeserializeObject<List<BlogCommentViewModel>>(content);
                    ViewData["comments"] = comments;
                }
                else
                {
                    ViewData["comments"] = new List<BlogCommentViewModel>();
                }

                return View(viewModel);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Comment(BlogCommentViewModel comment)
        {
            Console.WriteLine(comment.UserId);
            var response = await _apiService.PostAsync("/blog/comment/create", comment);
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
        [Route("/Blog/GetComments/{blogId}")]
        public async Task<IActionResult> GetComments(string blogId)
        {
            var response = await _apiService.GetAsync($"/blog/comment/get-all/{blogId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var comments = JsonConvert.DeserializeObject<List<BlogCommentViewModel>>(content);
                return PartialView("Components/_BlogCommentList", comments);
            }
            else
            {
                return NoContent();
            }
        }
    }
}
