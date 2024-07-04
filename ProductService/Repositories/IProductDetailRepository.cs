using ProductService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Repositories
{
    public interface IProductDetailRepository
    {
        Task<int> CreateOne(ProductDetail productDetail);
        Task<int> Remove(ProductDetail productDetail);
        Task<ProductDetail> FindById(int id);
    }
}
