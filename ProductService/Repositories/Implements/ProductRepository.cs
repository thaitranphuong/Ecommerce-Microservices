using Microsoft.EntityFrameworkCore;
using ProductService.Models;
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
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductDetails)
                .Include(p => p.Comments)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(p => p.Id == id);
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

        public async Task<List<Product>> FindByNameAndCategoryIdAndPrice(string name, int categoryId, float price)
        {
            IQueryable<Product> result1;
            if (string.IsNullOrEmpty(name))
                result1 = _context.Products.Where(p => p.Price >= price && p.Enabled == true).Include(p => p.Category);
            else
                result1 = _context.Products.Where(p => p.Name.Contains(name) && p.Price >= price && p.Enabled == true).Include(p => p.Category);

            List<Product> result2;
            if (categoryId == 0)
                result2 = await result1.ToListAsync();
            else
                result2 = await result1.Where(p => p.CategoryId == categoryId).ToListAsync();
            return result2;
        }

        public async Task<List<Product>> FindAll(string name, int categoryId, float price, int page, int limit)
        {
            IQueryable<Product> result1;
            if (string.IsNullOrEmpty(name))
                result1 = _context.Products.Where(p => p.Price >= price && p.Enabled == true).Include(p => p.Category);
            else
                result1 = _context.Products.Where(p => p.Name.Contains(name) && p.Price >= price && p.Enabled == true).Include(p => p.Category);

            List<Product> result2;
            if (categoryId == 0)
                result2 = await result1.Skip((page - 1) * limit).Take(limit).ToListAsync();
            else
                result2 = await result1.Where(p => p.CategoryId == categoryId).Skip((page - 1) * limit).Take(limit).ToListAsync();
            return result2;
        }
    }
}
