using Microsoft.EntityFrameworkCore;
using ProductService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Repositories.Implements
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateOne(Category category)
        {
            await _context.Categories.AddAsync(category);
            return await _context.SaveChangesAsync();
        }

        public async Task<Category> FindById(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<List<Category>> FindAll(string name, int page, int limit)
        {
            if(string.IsNullOrEmpty(name)) 
                return await _context.Categories
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            return await _context.Categories
                .Where(c => c.Name.Contains(name))
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<int> SaveChange()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Remove(Category category)
        {
            _context.Categories.Remove(category);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Category>> FindByName(string name)
        {
            if(string.IsNullOrEmpty(name))
                return await _context.Categories
                .ToListAsync();

            return await _context.Categories
                .Where(c => c.Name.Contains(name))
                .ToListAsync();
        }
    }
}
