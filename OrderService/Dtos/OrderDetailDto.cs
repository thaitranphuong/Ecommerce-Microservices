using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Dtos
{
    public class OrderDetailDto
    {
        public int ProductId { get; set; }

        public int OrderId { get; set; }

        public string Name { get; set; }

        public string Thumbnail { get; set; }

        public float Price { get; set; }

        public int Quantity { get; set; }
    }
}
