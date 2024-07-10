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
        Task<List<Comment>> FindAll(string userId, int productId, int page, int limit);
        Task<List<Comment>> FindByUserIdAndProductId(string userId, int productId);
    }
}
