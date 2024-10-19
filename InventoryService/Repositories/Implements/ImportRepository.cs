using Microsoft.EntityFrameworkCore;
using InventoryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Repositories.Implements
{
    public class ImportRepository : IImportRepository
    {
        private readonly AppDbContext _context;

        public ImportRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateOne(Import import)
        {
            await _context.Imports.AddAsync(import);
            await _context.SaveChangesAsync();
            return import.Id;
        }

        public async Task<Import> FindById(int id)
        {
            return await _context.Imports
                .Include(i => i.ImportDetails)
                .Include(i => i.Supplier)
                .Include(i => i.Warehouse)
                .FirstOrDefaultAsync(I => I.Id == id);
        }

        public async Task<List<Import>> FindAll(DateTime startTime, DateTime endTime, int page, int limit)
        {
            return await _context.Imports
                .Include(i => i.ImportDetails)
                .Include(i => i.Supplier)
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

        public async Task<List<Import>> FindAllByDate(DateTime startTime, DateTime endTime)
        {
            return await _context.Imports
                .Include(i => i.ImportDetails)
                .Include(i => i.Supplier)
                .Include(i => i.Warehouse)
                .Where(i => i.CreatedTime >= startTime && i.CreatedTime <= endTime)
                .OrderBy(i => i.CreatedTime)
                .ToListAsync();
        }

        public async Task<List<Import>> FindByWarehouseId(int id)
        {
            return await _context.Imports
               .Include(i => i.ImportDetails)
               .Include(i => i.Supplier)
               .Include(i => i.Warehouse)
               .Where(i => i.WarehouseId == id)
               .ToListAsync();
        }

        public async Task<List<ImportDetail>> FindAllByProductId(int id)
        {
            return await _context.ImportDetails
               .Include(i => i.Import)
               .ThenInclude(i => i.Warehouse)
               .Where(i => i.ProductId == id)
               .ToListAsync();
        }
    }
}
