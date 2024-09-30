using IdentityService.Models.DTOs;
using IdentityService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace IdentityService.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;
        public ChatHub(IMessageService messageService)
        {
            _messageService = messageService;
        }
        public async Task SendMessageToUser(string receiverId, MessageDto message)
        {
            await Clients.User(receiverId).SendAsync("/user-chat/" + receiverId + "/private", message);
            await _messageService.Save(message);

        }
    }
}
