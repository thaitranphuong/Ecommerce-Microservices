using IdentityService.Models;
using IdentityService.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityService.Repositories
{
    public interface IMessageRepository
    {
        Task<List<Message>> FindAll(string userIdFirst, string userIdSecond);
        Task Save(Message message);
    }
}
