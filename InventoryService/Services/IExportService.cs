using InventoryService.Dtos;
using InventoryService.Dtos.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Services
{
    public interface IExportService
    {
        Task<bool> Save(ExportDto export);
        Task<ExportDto> FindById(int id);
        Task<ExportOutput> FindAll(DateTime startTime, DateTime endTime, int page, int limit);
    }
}
