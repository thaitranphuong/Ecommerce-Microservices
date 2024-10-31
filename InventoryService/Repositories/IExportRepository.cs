using InventoryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Repositories
{
    public interface IExportRepository
    {
        Task<int> CreateOne(Export export);
        Task<Export> FindById(int id);
        Task<List<Export>> FindByWarehouseId(int id);
        Task<List<Export>> FindAllByDate(DateTime startTime, DateTime endTime);
        Task<List<Export>> FindAll(DateTime startTime, DateTime endTime, int page, int limit);
        Task<List<ExportDetail>> FindAllByProductId(int id);
        Task<int> SaveChange();
    }
}
