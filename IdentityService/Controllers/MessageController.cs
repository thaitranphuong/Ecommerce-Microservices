using IdentityService.Hubs;
using IdentityService.Models.DTOs;
using IdentityService.Services;
using IdentityService.Services.Implements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [EnableCors("AllowAll")]
    //[Authorize(Roles = "admin,customer")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IFileStorageService _fileStorageService;
        public MessageController(IMessageService messageService, IFileStorageService fileStorageService)
        {
            _messageService = messageService;
            _fileStorageService = fileStorageService;
        }

        [HttpGet("get-all/{userIdFirst}/{userIdSecond}")]
        public async Task<ActionResult<List<MessageDto>>> GetAllMessage(string userIdFirst, string userIdSecond)
        {
            return await _messageService.FindAll(userIdFirst, userIdSecond);
        }

        [HttpPost]
        [Route("upload-message-image")]
        public async Task<IActionResult> UploadMessageImage(IFormFile image)
        {
            var filePath = await _fileStorageService.Upload("message", image);
            if (filePath != null) return Ok(new { path = filePath });
            else return StatusCode(500);
        }

    }
}
