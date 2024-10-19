using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace InventoryService.SyncServices
{
    public class GrpcOrderDetailService : IGrpcOrderDetailService
    {
        private readonly GrpcChannel channel;
        private readonly GrpcOrderDetail.GrpcOrderDetailClient client;
        private readonly IConfiguration _configuration;

        public GrpcOrderDetailService(IConfiguration configuration)
        {
            _configuration = configuration;
            channel = GrpcChannel.ForAddress(_configuration["GrpcUrl:OrderService"]);
            client = new GrpcOrderDetail.GrpcOrderDetailClient(channel);
        }
        public async Task<OrderDetailResponse> GetOrderDetail(int orderId, int productId, int warehouseId)
        {
            var reply = await client.GetOrderDetailAsync(new OrderDetailRequest { OrderId = orderId, ProductId = productId, WarehouseId = warehouseId });
            return reply;
        }
    }
}
