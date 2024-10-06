using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Dtos
{
    public class CommentDto
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public int Star { get; set; }

        public DateTime CreatedTime { get; set; }

        public string Image { get; set; }

        public int ProductId { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string UserAvatar { get; set; }

        public int LikeCount { get; set; }

        public List<string> UserLikedIds { get; set; }
    }
}
