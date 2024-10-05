using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace InventoryService.SyncServices
{
    public class GrpcProductService : IGrpcProductService
    {
        private readonly GrpcChannel channel;
        private readonly GrpcProduct.GrpcProductClient client;
        private readonly IConfiguration _configuration;

        public GrpcProductService(IConfiguration configuration)
        {
            _configuration = configuration;
            channel = GrpcChannel.ForAddress(_configuration["GrpcUrl:ProductService"]);
            client = new GrpcProduct.GrpcProductClient(channel);
        }

        public async Task<ProductResponse> GetProduct(int productId)
        {
            var reply = await client.GetProductAsync(new ProductRequest { Id = productId });
            return reply;
        }
    }
}
