﻿using OrderService.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }

        public string CustomerName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public OrderStatus Status { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public string TransportMethod { get; set; }

        public string Note { get; set; }

        public float TransportFee { get; set; }

        public float Total { get; set; }

        public int VoucherId { get; set; }

        public string VoucherName { get; set; }

        public float VoucherDiscountPercent { get; set; }

        public float VoucherMaxDiscount { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public ICollection<OrderDetailDto> OrderDetails { get; set; }
    }
}
