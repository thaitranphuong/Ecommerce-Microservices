using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductService.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string ShortDescription { get; set; }

        [DataType(DataType.Text)]
        public string FullDescription { get; set; }

        public string Thumbnail { get; set; }

        public float Price { get; set; }

        public int DiscountPercent { get; set; }

        public int Quantity { get; set; }

        public int SoldQuantity { get; set; }

        public string Origin { get; set; }

        public int Expiry { get; set; }

        public bool Enabled { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<ProductDetail> ProductDetails { get; set; }
    }
}
