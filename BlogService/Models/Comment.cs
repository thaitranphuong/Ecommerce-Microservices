using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogService.Models
{
    public class Comment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string ExternalId { get; set; }

        public string Content { get; set; }

        public DateTime CreatedTime { get; set; }

        public string UserId { get; set; }

        public string BlogId { get; set; }
    }
}
