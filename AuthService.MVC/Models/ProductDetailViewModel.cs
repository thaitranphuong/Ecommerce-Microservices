using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.MVC.Models
{
    public class ProductDetailViewModel
    {
        public int Id { get; set; }

        public string Image { get; set; }

        public int ProductId { get; set; }
    }
}
