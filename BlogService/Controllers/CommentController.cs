using BlogService.Dtos;
using BlogService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        [Route("get-all/{blogId}")]
        public async Task<IActionResult> GetAll([FromRoute] string blogId)
        {
            var dtos = await _commentService.FindByBlogId(blogId);
            return Ok(dtos);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(CommentDto dto)
        {
            bool result = await _commentService.Save(dto);
            if (result) return Ok(dto);
            else return StatusCode(500);
        }
    }
}
