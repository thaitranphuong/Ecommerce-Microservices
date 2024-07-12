using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogService.SyncServices
{
    public interface IGrpcUserService
    {
        Task<UserResponse> GetUser(string userId);
    }
}
