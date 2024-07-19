using System.Threading.Tasks;

namespace OrderService.SyncServices
{
    public interface IGrpcProductService
    {
        Task<ProductResponse> GetProduct(int product);
    }
}
