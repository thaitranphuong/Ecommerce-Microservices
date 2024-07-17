using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Constants
{
    public enum OrderStatus
    {
        ALL,
        PENDING,
        DELIVERING,
        RECEIVED,
        CANCELED
    }
}
