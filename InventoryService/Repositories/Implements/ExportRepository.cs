using Microsoft.EntityFrameworkCore;
using InventoryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Repositories.Implements
{
    public class ExportRepository : IExportRepository
    {
        private readonly AppDbContext _context;

        public ExportRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateOne(Export export)
        {
            await _context.Exports.AddAsync(export);
            await _context.SaveChangesAsync();
            return export.Id;
        }

        public async Task<Export> FindById(int id)
        {
            return await _context.Exports
                .Include(i => i.ExportDetails)
                .Include(i => i.Warehouse)
                .FirstOrDefaultAsync(I => I.Id == id);
        }

        public async Task<List<Export>> FindAll(DateTime startTime, DateTime endTime, int page, int limit)
        {
            return await _context.Exports
                .Include(i => i.ExportDetails)
                .Include(i => i.Warehouse)
                .Where(i => i.CreatedTime >= startTime && i.CreatedTime <= endTime)
                .OrderBy(i => i.CreatedTime)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<int> SaveChange()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Export>> FindAllByDate(DateTime startTime, DateTime endTime)
        {
            return await _context.Exports
                .Include(i => i.ExportDetails)
                .Include(i => i.Warehouse)
                .Where(i => i.CreatedTime >= startTime && i.CreatedTime <= endTime)
                .OrderBy(i => i.CreatedTime)
                .ToListAsync();
        }

        public async Task<List<Export>> FindByWarehouseId(int id)
        {
            return await _context.Exports
               .Include(i => i.ExportDetails)
               .Include(i => i.Warehouse)
               .Where(i => i.WarehouseId == id)
               .ToListAsync();
        }

        public async Task<List<ExportDetail>> FindAllByProductId(int id)
        {
            return await _context.ExportDetails
               .Include(i => i.Export)
               .ThenInclude(i => i.Warehouse)
               .Where(i => i.ProductId == id)
               .ToListAsync();
        }
    }
}
