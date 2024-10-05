using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.SyncServices
{
    public interface IGrpcProductService
    {
        Task<ProductResponse> GetProduct(int productId);
    }
}
