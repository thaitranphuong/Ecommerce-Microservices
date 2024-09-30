using IdentityService.Models;
using IdentityService.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Repositories.Implements
{
    public class MessageRepository : IMessageRepository
    {
        private readonly MyDbContext _context;
        public MessageRepository(MyDbContext context)
        {
            _context = context;
        }
        public async Task<List<Message>> FindAll(string userIdFirst, string userIdSecond)
        {
            return await _context.Messages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Where(m => (m.Sender.Id == userIdFirst && m.Receiver.Id == userIdSecond) ||
                    (m.Sender.Id == userIdSecond && m.Receiver.Id == userIdFirst))
                .ToListAsync();

        }

        public async Task Save(Message message)
        {
           _context.Messages.Add(message);
           await _context.SaveChangesAsync();
        }
    }
}
