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
        Task<bool> UpdateViewNumber(string id);
        Task<BlogDto> FindById(string id);
        Task<BlogDto> FindBySlug(string slug);
        Task<BlogOutput> FindAll(string title, int page, int limit);
        Task<bool> DeleteById(string id);
    }
}
