using OrderService.Constants;
using OrderService.Dtos.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Dtos.Paginations
{
    public class OrderOutput : AbstractOutput<OrderDto>
    {
        public OrderStatus Status;
    }
}
