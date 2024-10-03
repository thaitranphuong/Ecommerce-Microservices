using Microsoft.EntityFrameworkCore;
using InventoryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Repositories.Implements
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly AppDbContext _context;

        public SupplierRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateOne(Supplier supplier)
        {
            await _context.Suppliers.AddAsync(supplier);
            return await _context.SaveChangesAsync();
        }

        public async Task<Supplier> FindById(int id)
        {
            return await _context.Suppliers.FindAsync(id);
        }

        public async Task<List<Supplier>> FindAll(string name, int page, int limit)
        {
            if (string.IsNullOrEmpty(name))
                return await _context.Suppliers
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            return await _context.Suppliers
                .Where(c => c.Name.Contains(name))
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<int> SaveChange()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Remove(Supplier supplier)
        {
            _context.Suppliers.Remove(supplier);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Supplier>> FindByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return await _context.Suppliers
                .ToListAsync();

            return await _context.Suppliers
                .Where(c => c.Name.Contains(name))
                .ToListAsync();
        }
    }
}
