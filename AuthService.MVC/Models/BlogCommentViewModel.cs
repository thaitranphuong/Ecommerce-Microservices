using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.MVC.Models
{
    public class BlogCommentViewModel
    {
        public string Id { get; set; }

        public string ExternalId { get; set; }

        public string Content { get; set; }

        public DateTime CreatedTime { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string UserAvatar { get; set; }

        public string BlogId { get; set; }
    }
}
