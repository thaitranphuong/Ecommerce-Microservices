using IdentityService.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityService.Services
{
    public interface IMessageService
    {
        Task<List<MessageDto>> FindAll(string userIdFirst, string userIdSecond);
        Task Save(MessageDto message);
    }

}
