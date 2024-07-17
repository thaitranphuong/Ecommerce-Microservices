using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.MVC.Models
{
    public class CheckoutViewModel
    {
        public OrderViewModel Order { get; set; }
        public ICollection<OrderDetailViewModel> OrderDetails { get; set; }
    }
}
