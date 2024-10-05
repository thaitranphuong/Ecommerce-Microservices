using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace InventoryService.SyncServices
{
    public class GrpcUserService : IGrpcUserService
    {
        private readonly GrpcChannel channel;
        private readonly GrpcUser.GrpcUserClient client;
        private readonly IConfiguration _configuration;

        public GrpcUserService(IConfiguration configuration)
        {
            _configuration = configuration;
            channel = GrpcChannel.ForAddress(_configuration["GrpcUrl:IdentityService"]);
            client = new GrpcUser.GrpcUserClient(channel);
        }

        public async Task<UserResponse> GetUser(string userId)
        {
            var reply = await client.GetUserAsync(new UserRequest { Id = userId });
            return reply;
        }
    }
}
