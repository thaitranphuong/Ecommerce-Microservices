using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductService.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }

        public string Avatar { get; set; }

        public ICollection<Comment> Commnents { get; set; }
    }
}
