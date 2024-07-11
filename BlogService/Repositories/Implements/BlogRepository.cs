using BlogService.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogService.Repositories.Implements
{
    public class BlogRepository : IBlogRepository
    {
        private readonly AppDbContext _context;

        public BlogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateOne(Blog blog)
        {
            await _context.Blogs.InsertOneAsync(blog);
            return 1;
        }

        public async Task<Blog> FindById(string id)
        {
            var filter = Builders<Blog>.Filter.Eq(b => b.ExternalId, id);
            return await _context.Blogs.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<Blog>> FindAll(string title, int page, int limit)
        {
            var filter = Builders<Blog>.Filter.Empty;

            if (!string.IsNullOrEmpty(title))
            {
                filter = Builders<Blog>.Filter.Regex("Title", new MongoDB.Bson.BsonRegularExpression(title, "i"));
            }

            return await _context.Blogs.Find(filter)
                                    .Skip((page - 1) * limit)
                                    .Limit(limit)
                                    .ToListAsync();
        }

        public async Task<int> Update(Blog blog)
        {
            var filter = Builders<Blog>.Filter.Eq(b => b.ExternalId, blog.ExternalId);
            var update = Builders<Blog>.Update
                                            .Set(b => b.Title, blog.Title)
                                            .Set(b => b.Thumbnail, blog.Thumbnail)
                                            .Set(b => b.ShortDescription, blog.ShortDescription)
                                            .Set(b => b.Content, blog.Content)
                                            .Set(b => b.Slug, blog.Slug)
                                            .Set(b => b.UpdatedTime, blog.UpdatedTime);

            return (int)(await _context.Blogs.UpdateOneAsync(filter, update)).ModifiedCount;
        }

        public async Task<int> Remove(string id)
        {
           return (int)(await _context.Blogs.DeleteOneAsync(b => b.ExternalId == id)).DeletedCount;
        }

        public async Task<List<Blog>> FindByTitle(string title)
        {
            var filter = Builders<Blog>.Filter.Empty;

            if (!string.IsNullOrEmpty(title))
            {
                filter = Builders<Blog>.Filter.Regex("Title", new MongoDB.Bson.BsonRegularExpression(title, "i"));
            }

            return await _context.Blogs.Find(filter).ToListAsync();
        }
    }
}
