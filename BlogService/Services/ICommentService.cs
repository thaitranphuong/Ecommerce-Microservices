using BlogService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogService.Services
{
    public interface ICommentService
    {
        Task<bool> Save(CommentDto comment);
        Task<List<CommentDto>> FindByBlogId(string blogId);
    }
}
