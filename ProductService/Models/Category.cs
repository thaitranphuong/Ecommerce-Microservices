using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductService.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
