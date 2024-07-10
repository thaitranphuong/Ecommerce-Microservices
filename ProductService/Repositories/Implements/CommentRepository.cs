using Microsoft.EntityFrameworkCore;
using ProductService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Repositories.Implements
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
            await _context.Comments.AddAsync(comment);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Comment>> FindAll(string userId, int productId, int page, int limit)
        {
            return await _context.Comments
               .Where(c => c.UserId == userId && c.ProductId == productId)
               .Skip((page - 1) * limit)
               .Take(limit)
               .ToListAsync();
        }

        public async Task<List<Comment>> FindByUserIdAndProductId(string userId, int productId)
        {
            return await _context.Comments
                .Where(c => c.UserId == userId && c.ProductId == productId)
                .ToListAsync();
        }
    }
}
