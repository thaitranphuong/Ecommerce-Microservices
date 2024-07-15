using System.Threading.Tasks;

namespace CartService.SyncServices
{
    public interface IGrpcProductService
    {
        Task<ProductResponse> GetProduct(int product);
    }
}
