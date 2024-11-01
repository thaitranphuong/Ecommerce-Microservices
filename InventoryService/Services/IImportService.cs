using InventoryService.Dtos;
using InventoryService.Dtos.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Services
{
    public interface IImportService
    {
        Task<bool> Save(ImportDto import);
        Task<ImportDto> FindById(int id);
        Task<bool> UpdateImportDetail(List<ImportDetailDto> importDetailDtos);
        Task<ImportOutput> FindAll(DateTime startTime, DateTime endTime, int page, int limit);
        Task<List<ItemLineDto>> GetAllItemLines(int warehouseId);
    }
}
