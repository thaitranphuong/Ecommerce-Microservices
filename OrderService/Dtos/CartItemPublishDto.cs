using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Dtos
{
    public class CartItemPublishDto
    {
        public string UserId { get; set; }
        public int ProductId { get; set; }
    }
}
