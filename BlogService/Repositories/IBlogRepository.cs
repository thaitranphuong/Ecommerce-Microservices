using BlogService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogService.Repositories
{
    public interface IBlogRepository
    {
        Task<int> CreateOne(Blog blog);
        Task<Blog> FindById(string id);
        Task<List<Blog>> FindByTitle(string title);
        Task<List<Blog>> FindAll(string title, int page, int limit);
        Task<int> Update(Blog blog);
        Task<int> Remove(string id);
    }
}
