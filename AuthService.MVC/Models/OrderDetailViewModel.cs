using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.MVC.Models
{
    public class OrderDetailViewModel
    {
        public int ProductId { get; set; }

        public int OrderId { get; set; }

        public string Name { get; set; }

        public string Thumbnail { get; set; }

        public float Price { get; set; }

        public int Quantity { get; set; }
    }
}
