using System.Threading.Tasks;

namespace OrderService.SyncServices
{
    public interface IGrpcUserService
    {
        Task<UserResponse> GetUser(string userId);
    }
}

