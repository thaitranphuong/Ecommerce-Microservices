using AuthService.MVC.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.MVC.Models.Pagination
{
    public class OrderOutput : AbtractOutput<OrderViewModel>
    {
        public OrderStatus Status;
    }
}
