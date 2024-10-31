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
        Task<List<Product>> FindByNameAndCategoryIdAndPrice(string name, int categoryId, int unitId, float price);
        Task<List<Product>> FindAll(string name, int page, int limit);
        Task<List<Product>> FindAll(string name, int categoryId, int unitId, float price, int page, int limit);
        Task<List<Product>> FindAllByOrderBySoldquantity(int page, int limit);
        Task<int> SaveChange();

    }
}
