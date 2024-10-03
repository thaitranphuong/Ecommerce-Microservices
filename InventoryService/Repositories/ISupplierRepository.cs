using InventoryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Repositories
{
    public interface ISupplierRepository
    {
        Task<int> CreateOne(Supplier supplier);
        Task<Supplier> FindById(int id);
        Task<List<Supplier>> FindByName(string name);
        Task<List<Supplier>> FindAll(string name, int page, int limit);
        Task<int> SaveChange();
        Task<int> Remove(Supplier supplier);
    }
}
