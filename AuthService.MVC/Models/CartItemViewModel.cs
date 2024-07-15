using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.MVC.Models
{
    public class CartItemViewModel
    {
        public string UserId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public string Name { get; set; }

        public string Thumbnail { get; set; }

        public float Price { get; set; }
    }
}
