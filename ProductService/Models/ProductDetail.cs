using System.ComponentModel.DataAnnotations;

namespace ProductService.Models
{
    public class ProductDetail
    {
        [Key]
        public int Id { get; set; }

        public string Image { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
