using InventoryService.Dtos;
using InventoryService.Dtos.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Services
{
    public interface ISupplierService
    {
        Task<bool> Save(SupplierDto supplier);
        Task<SupplierDto> FindById(int id);
        Task<SupplierOutput> FindAll(string name, int page, int limit);
        Task<bool> DeleteById(int id);
    }
}
