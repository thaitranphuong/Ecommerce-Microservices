using ProductService.Dtos;
using ProductService.Dtos.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Services
{
    public interface IProductService
    {
        Task<int> Save(ProductDto product);
        Task<int> SaveShowHide(int id);
        Task<ProductDto> FindById(int id);
        Task<ProductOutput> FindAll(string name, int page, int limit);
        Task<ProductOutput> FindAll(string name, int categoryId, float price,  int page, int limit);
        Task<ProductOutput> FindAllByOrderBySoldquantity(int page, int limit);
    }
}
