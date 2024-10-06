using Microsoft.AspNetCore.Http;
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
        private readonly IFileStorageService _fileStorageService;

        public CommentController(ICommentService commentService, IFileStorageService fileStorageService)
        {
            _commentService = commentService;
            _fileStorageService = fileStorageService;
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

        [HttpPost]
        [Route("upload-comment-image")]
        public async Task<IActionResult> UploadCommentImage(IFormFile image)
        {
            var filePath = await _fileStorageService.Upload("comments", image);
            if (filePath != null) return Ok(new { path = filePath });
            else return StatusCode(500);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _commentService.DeleteById(id);
            if (result) return Ok(new { status = 200 });
            else return StatusCode(500);
        }

        [HttpGet]
        [Route("like/{commentId}/{userId}")]
        public async Task<IActionResult> Like(int commentId, string userId)
        {
            var result = await _commentService.LikeOrUnLike(true, commentId, userId);
            if (result) return Ok(new { status = 200 });
            else return StatusCode(500);
        }

        [HttpGet]
        [Route("unlike/{commentId}/{userId}")]
        public async Task<IActionResult> UnLike(int commentId, string userId)
        {
            var result = await _commentService.LikeOrUnLike(false, commentId, userId);
            if (result) return Ok(new { status = 200 });
            else return StatusCode(500);
        }
    }
}
