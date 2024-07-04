using ProductService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Repositories.Implements
{
    public class ProductDetailRepository : IProductDetailRepository
    {
        private readonly AppDbContext _context;

        public ProductDetailRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateOne(ProductDetail productDetail)
        {
            await _context.ProductDetails.AddAsync(productDetail);
            return await _context.SaveChangesAsync();
        }

        public async Task<ProductDetail> FindById(int id)
        {
            return await _context.ProductDetails.FindAsync(id);
        }

        public async Task<int> Remove(ProductDetail productDetail)
        {
            _context.ProductDetails.Remove(productDetail);
            return await _context.SaveChangesAsync();
        }
    }
}
