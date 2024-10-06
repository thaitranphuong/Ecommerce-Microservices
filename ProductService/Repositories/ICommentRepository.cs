using ProductService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Repositories
{
    public interface ICommentRepository
    {
        Task<int> CreateOne(Comment comment);
        Task<List<Comment>> FindAll(int productId, int page, int limit);
        Task<List<Comment>> FindByProductId(int productId);
        Task<int> Remove(Comment comment);
        Task<Comment> FindById(int id);
        Task<int> SaveChange();
    }
}
