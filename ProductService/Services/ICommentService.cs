using ProductService.Dtos;
using ProductService.Dtos.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Services
{
    public interface ICommentService
    {
        Task<bool> Save(CommentDto comment);
        Task<CommentOutput> FindAll(int productId, int page, int limit);
    }
}
