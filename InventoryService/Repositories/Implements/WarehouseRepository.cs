using Microsoft.EntityFrameworkCore;
using InventoryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Repositories.Implements
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly AppDbContext _context;

        public WarehouseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateOne(Warehouse warehouse)
        {
            await _context.Warehouses.AddAsync(warehouse);
            return await _context.SaveChangesAsync();
        }

        public async Task<Warehouse> FindById(int id)
        {
            return await _context.Warehouses.FindAsync(id);
        }

        public async Task<List<Warehouse>> FindAll(string name, int page, int limit)
        {
            if(string.IsNullOrEmpty(name)) 
                return await _context.Warehouses
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            return await _context.Warehouses
                .Where(c => c.Name.Contains(name))
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<int> SaveChange()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Remove(Warehouse warehouse)
        {
            _context.Warehouses.Remove(warehouse);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Warehouse>> FindByName(string name)
        {
            if(string.IsNullOrEmpty(name))
                return await _context.Warehouses
                .ToListAsync();

            return await _context.Warehouses
                .Where(c => c.Name.Contains(name))
                .ToListAsync();
        }
    }
}
