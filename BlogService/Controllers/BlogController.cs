using BlogService.Dtos;
using BlogService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace BlogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;
        private readonly IFileStorageService _fileStorageService;

        public BlogController(IBlogService blogService, IFileStorageService fileStorageService)
        {
            _blogService = blogService;
            _fileStorageService = fileStorageService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(IFormFile thumbnail)
        {
            var formData = await Request.ReadFormAsync();
            var json = formData["blog"];

            var blog = JsonConvert.DeserializeObject<BlogDto>(json);
            if (thumbnail != null)
            {
                var filePath = await _fileStorageService.Upload("blog", thumbnail);
                blog.Thumbnail = filePath;
            }
            bool result = await _blogService.Save(blog);
            if (result) return Ok(result);
            else return StatusCode(500);
        }

        [HttpPost]
        [Route("upload-file")]
        public async Task<IActionResult> UploadFile(IFormFile image)
        {
            var filePath = await _fileStorageService.Upload("ckeditor", image);
            if (filePath != null) return Ok(new { path = filePath });
            else return StatusCode(500);
        }

        [HttpGet]
        [Route("get/{blogId}")]
        public async Task<IActionResult> Get(string blogId)
        {
            BlogDto dto = await _blogService.FindById(blogId);
            if (dto != null) return Ok(dto);
            else return StatusCode(404);
        }

        [HttpGet]
        [Route("get-by-slug/{slug}")]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            BlogDto dto = await _blogService.FindBySlug(slug);
            if (dto != null) return Ok(dto);
            else return StatusCode(404);
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAll([FromQuery] int page,
                                                [FromQuery] int limit, [FromQuery] string title)
        {
            var dtos = await _blogService.FindAll(title, page, limit);
            return Ok(dtos);
        }

        [HttpGet]
        [Route("get-all-order-by-views")]
        public async Task<IActionResult> GetAllOrderByView()
        {
            var dtos = await _blogService.FindAllOrderByView();
            return Ok(dtos);
        }

        [HttpGet]
        [Route("update-view-number/{blogId}")]
        public async Task<IActionResult> UpdateViewNumber(string blogId)
        {
            await _blogService.UpdateViewNumber(blogId);
            return Ok(new { StatusCode = 200});
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update(IFormFile thumbnail)
        {
            var formData = await Request.ReadFormAsync();
            var json = formData["blog"];
            var blog = JsonConvert.DeserializeObject<BlogDto>(json);
            if (thumbnail != null)
            {
                var filePath = await _fileStorageService.Upload("blog", thumbnail);
                blog.Thumbnail = filePath;
            }
            bool result = await _blogService.Save(blog);
            if (result) return Ok(result);
            else return StatusCode(500);
        }

        [HttpDelete]
        [Route("delete/{blogId}")]
        public async Task<IActionResult> Delete(string blogId)
        {
            bool result = await _blogService.DeleteById(blogId);
            if (result) return Ok(new { message = "OK"});
            else return StatusCode(500);
        }
    }
}
