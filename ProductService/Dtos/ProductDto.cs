using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Unit { get; set; }

        public string ShortDescription { get; set; }

        public string FullDescription { get; set; }

        public string Thumbnail { get; set; }

        public float Price { get; set; }

        public float OldPrice { get; set; }

        public int DiscountPercent { get; set; }

        public int Quantity { get; set; }

        public int SoldQuantity { get; set; }

        public string Origin { get; set; }

        public int Expiry { get; set; }

        public bool Enabled { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public ICollection<ProductDetailDto> ProductDetails { get; set; }

        public ICollection<CommentDto> Comments { get; set; }
    }
}
