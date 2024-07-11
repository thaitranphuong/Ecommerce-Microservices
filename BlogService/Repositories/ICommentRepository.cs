using BlogService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogService.Repositories
{
    public interface ICommentRepository
    {
        Task<int> CreateOne(Comment comment);
        Task<List<Comment>> FindByBlogId(string blogId);
    }
}
