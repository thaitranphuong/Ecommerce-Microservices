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
    [EnableCors("AllowAll")]
    [Authorize(Roles = "admin,customer")]
    public class MessageController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IMessageService _messageService;
        public MessageController(IHubContext<ChatHub> hubContext, IMessageService messageService)
        {
            _hubContext = hubContext;
            _messageService = messageService;
        }

        [HttpGet("get-all/{userIdFirst}/{userIdSecond}")]
        public async Task<ActionResult<List<MessageDto>>> GetAllMessage(string userIdFirst, string userIdSecond)
        {
            return await _messageService.FindAll(userIdFirst, userIdSecond);
        }

        [HttpPost("private-message")]
        public async Task<IActionResult> PrivateReceiveMessage([FromBody] MessageDto messageDTO)
        {
            await _hubContext.Clients.User(messageDTO.ReceiverId.ToString())
                .SendAsync("/user-chat/" + messageDTO.ReceiverId + "/private", messageDTO);
            await _messageService.Save(messageDTO);
            return Ok();
        }
    }
}
