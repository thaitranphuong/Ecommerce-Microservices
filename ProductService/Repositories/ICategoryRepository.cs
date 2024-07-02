using ProductService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Repositories
{
    public interface ICategoryRepository
    {
        Task<int> CreateOne(Category category);
        Task<Category> FindById(int id);
        Task<List<Category>> FindByName(string name);
        Task<List<Category>> FindAll(string name, int page, int limit);
        Task<int> SaveChange();
        Task<int> Remove(Category category);
    }
}
