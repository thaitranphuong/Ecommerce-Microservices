using IdentityService.Models.DTOs;
using IdentityService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;
using System;
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
        public async Task SendPrivateMessage(string receiverId, MessageDto message)
        {
            await Clients.All.SendAsync("ReceivePrivateMessage/" + receiverId, message);
            await _messageService.Save(message);
        }

        public override async Task OnConnectedAsync()
        {
            string userId = Context.UserIdentifier;
            // Logic for handling connection
            await base.OnConnectedAsync();
            Console.WriteLine($"On connected with id: {userId}");
        }

        // Handle user disconnection
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // Logic for handling disconnection
            await base.OnDisconnectedAsync(exception);
            Console.WriteLine($"On disconnected");
        }
    }
}
