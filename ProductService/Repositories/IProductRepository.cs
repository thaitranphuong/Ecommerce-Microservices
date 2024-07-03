using ProductService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Repositories
{
    public interface IProductRepository
    {
        Task<int> CreateOne(Product product);
        Task<Product> FindById(int id);
        Task<List<Product>> FindByName(string name);
        Task<List<Product>> FindAll(string name, int page, int limit);
        Task<int> SaveChange();
    }
}
