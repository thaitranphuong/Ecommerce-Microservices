using AuthService.MVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthService.MVC.Hubs
{
    public class ChatHub : Hub
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ChatHub(AppDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task SendMessage(string receiverId, string message)
        {
            var senderId = _userManager.GetUserId(Context.User);

            var newMessage = new Message
            {
                Content = message,
                SenderId = senderId,
                ReceiverId = receiverId,
                Sender = _context.Users.Find(senderId),
                Receiver = _context.Users.Find(receiverId),
                CreatedTime = DateTime.Now
            };

            await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, newMessage);

            _context.Messages.Add(newMessage);
            await _context.SaveChangesAsync();
        }
    }
}
