using InventoryService.Dtos;
using InventoryService.Dtos.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Services
{
    public interface IWarehouseService
    {
        Task<bool> Save(WarehouseDto warehouse);
        Task<WarehouseDto> FindById(int id);
        Task<WarehouseOutput> FindAll(string name, int page, int limit);
        Task<bool> DeleteById(int id);
        Task<List<WarehouseDto>> FindAllToExport(int productId, int productQuantity);
        Task<List<InStockProductDto>> FindAllInstock(int id);
    }
}
