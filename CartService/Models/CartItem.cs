using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartService.Models
{
    public class CartItem
    {
        public string UserId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
