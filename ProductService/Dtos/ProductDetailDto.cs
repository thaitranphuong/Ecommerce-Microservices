using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Dtos
{
    public class ProductDetailDto
    {
        public int Id { get; set; }

        public string Image { get; set; }

        public int ProductId { get; set; }
    }
}
