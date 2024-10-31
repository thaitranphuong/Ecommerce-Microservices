using System.Collections;
using System.Collections.Generic;

namespace ProductService.Models
{
    public class Unit
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Product> Products { get; set;}
    }
}
