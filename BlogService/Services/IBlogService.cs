using BlogService.Dtos;
using BlogService.Dtos.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogService.Services
{
    public interface IBlogService
    {
        Task<bool> Save(BlogDto dto);
        Task<BlogDto> FindById(string id);
        Task<BlogOutput> FindAll(string title, int page, int limit);
        Task<bool> DeleteById(string id);
    }
}
