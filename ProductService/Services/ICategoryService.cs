using ProductService.Dtos;
using ProductService.Dtos.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Services
{
    public interface ICategoryService
    {
        Task<bool> Save(CategoryDto category);
        Task<CategoryDto> FindById(int id);
        Task<CategoryOutput> FindAll(string name, int page, int limit);
        Task<bool> DeleteById(int id);
    }
}
