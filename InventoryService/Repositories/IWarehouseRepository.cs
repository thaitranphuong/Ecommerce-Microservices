using InventoryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Repositories
{
    public interface IWarehouseRepository
    {
        Task<int> CreateOne(Warehouse warehouse);
        Task<Warehouse> FindById(int id);
        Task<List<Warehouse>> FindByName(string name);
        Task<List<Warehouse>> FindAll(string name, int page, int limit);
        Task<int> SaveChange();
        Task<int> Remove(Warehouse warehouse);
    }
}
