using InventoryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Repositories
{
    public interface IImportRepository
    {
        Task<int> CreateOne(Import import);
        Task<Import> FindById(int id);
        Task<ImportDetail> FindImportDetailById(int id);
        Task<List<Import>> FindByWarehouseId(int id);
        Task<List<Import>> FindAllByDate(DateTime startTime, DateTime endTime);
        Task<List<Import>> FindAll(DateTime startTime, DateTime endTime, int page, int limit);
        Task<List<ImportDetail>> FindAllByProductId(int id);
        Task<int> SaveChange();
    }
}
