using System;
using System.ComponentModel.DataAnnotations;

namespace ProductService.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [DataType(DataType.Text)]
        public string Content { get; set; }

        public int Star { get; set; }

        public DateTime CreatedTime { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
