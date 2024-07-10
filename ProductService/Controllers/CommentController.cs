using Microsoft.AspNetCore.Mvc;
using ProductService.Dtos;
using ProductService.Services;
using System.Threading.Tasks;

namespace ProductService.Controllers
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
        [Route("get-all/{productId}")]
        public async Task<IActionResult> GetAll([FromRoute] int productId,
                                                [FromQuery] int page,
                                                [FromQuery] int limit)
        {
            var dtos = await _commentService.FindAll(productId, page, limit);
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
