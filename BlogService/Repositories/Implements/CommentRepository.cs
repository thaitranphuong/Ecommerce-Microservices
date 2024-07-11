using BlogService.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogService.Repositories.Implements
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateOne(Comment comment)
        {
            await _context.Comments.InsertOneAsync(comment);
            return 1;
        }

        public async Task<List<Comment>> FindByBlogId(string blogId)
        {
            var filter = Builders<Comment>.Filter.Eq(c => c.BlogId, blogId);
            return await _context.Comments.Find(filter).ToListAsync();
        }
    }
}
