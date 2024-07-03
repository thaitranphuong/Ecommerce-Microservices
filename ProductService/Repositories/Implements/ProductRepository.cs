using Microsoft.EntityFrameworkCore;
using ProductService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Repositories.Implements
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateOne(Product product)
        {
            product.Enabled = true;
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }

        public async Task<Product> FindById(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<List<Product>> FindAll(string name, int page, int limit)
        {
            if(string.IsNullOrEmpty(name)) 
                return await _context.Products
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            return await _context.Products
                .Where(c => c.Name.Contains(name))
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<int> SaveChange()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> FindByName(string name)
        {
            if(string.IsNullOrEmpty(name))
                return await _context.Products
                .ToListAsync();

            return await _context.Products
                .Where(c => c.Name.Contains(name))
                .ToListAsync();
        }
    }
}
