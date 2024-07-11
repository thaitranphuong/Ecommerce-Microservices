﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogService.Dtos
{
    public class BlogDto
    {
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

        public ICollection<CommentDto> Comments { get; set; }

        public int CommentCount { get; set; }
    }
}