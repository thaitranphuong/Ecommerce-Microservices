using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogService.Models
{
    public class Blog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string ExternalId { get; set; }

        public string Title { get; set; }

        public string Thumbnail { get; set; }

        public string ShortDescription { get; set; }

        public string Content { get; set; }

        public string Slug { get; set; }

        public int ViewNumber { get; set; }

        public string AuthorName { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
