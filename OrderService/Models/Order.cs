using OrderService.Constants;
using System;
using System.Collections.Generic;

namespace OrderService.Models
{
    public class Order
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

        public float TransportFee { get; set; }

        public string Note { get; set; }

        public float Total { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }

        public int? VoucherId { get; set; }

        public Voucher Voucher { get; set; }

        public string UserId { get; set; }
    }
}
