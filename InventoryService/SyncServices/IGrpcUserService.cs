using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.SyncServices
{
    public interface IGrpcUserService
    {
        Task<UserResponse> GetUser(string userId);
    }
}
