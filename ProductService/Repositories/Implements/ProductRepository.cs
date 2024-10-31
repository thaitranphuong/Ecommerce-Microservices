using Microsoft.EntityFrameworkCore;
using ProductService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

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
                .Include(p => p.Unit_)
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
                .Include(p => p.Unit_)
                .Include(p => p.Category)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            return await _context.Products
                .Include(p => p.Unit_)
                .Include(p => p.Category)
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
                return await _context.Products.Include(p => p.Category).Include(p => p.Unit_)
                .ToListAsync();

            return await _context.Products
                .Where(c => c.Name.Contains(name)).Include(p => p.Category).Include(p => p.Unit_)
                .ToListAsync();
        }

        public async Task<List<Product>> FindByNameAndCategoryIdAndPrice(string name, int categoryId, int unitId, float price)
        {
            IQueryable<Product> result1;
            if (string.IsNullOrEmpty(name))
                result1 = _context.Products.OrderByDescending(p => p.Id).Where(p => p.Price >= price && p.Enabled == true).Include(p => p.Category).Include(p => p.Unit_);
            else
                result1 = _context.Products.OrderByDescending(p => p.Id).Where(p => p.Name.Contains(name) && p.Price >= price && p.Enabled == true).Include(p => p.Category).Include(p => p.Unit_);

            IQueryable<Product> result2;
            if (unitId == 0)
                result2 = result1;
            else
                result2 = result1.Where(p => p.UnitId == unitId);

            List<Product> result;
            if (categoryId == 0)
                result = await result2.ToListAsync();
            else
                result = await result2.Where(p => p.CategoryId == categoryId).ToListAsync();
            return result;
        }

        public async Task<List<Product>> FindAll(string name, int categoryId, int unitId, float price, int page, int limit)
        {
            IQueryable<Product> result1;
            if (string.IsNullOrEmpty(name))
                result1 = _context.Products.Where(p => p.Price >= price && p.Enabled == true).Include(p => p.Category).Include(p => p.Unit_);
            else
                result1 = _context.Products.Where(p => p.Name.Contains(name) && p.Price >= price && p.Enabled == true).Include(p => p.Category).Include(p => p.Unit_);

            IQueryable<Product> result2;
            if (unitId == 0)
                result2 = result1;
            else
                result2 = result1.Where(p => p.UnitId == unitId);

            List<Product> result;
            if (categoryId == 0)
                result = await result2.Skip((page - 1) * limit).Take(limit).ToListAsync();
            else
                result = await result2.Where(p => p.CategoryId == categoryId).Skip((page - 1) * limit).Take(limit).ToListAsync();
            return result;
        }

        public async Task<List<Product>> FindAllByOrderBySoldquantity(int page, int limit)
        {
            return await _context.Products.Include(p => p.Unit_)
                .Where(p => p.Enabled == true)
                .OrderByDescending(p => p.SoldQuantity)
                .Include(p => p.Category)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
        }
    }
}
