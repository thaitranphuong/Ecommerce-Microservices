using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Models
{
    public enum PaymentMethod
    {
        PENDING,
        DELIVERING,
        RECEIVED,
        CANCELED
    }

    public class Order
    {
        public int Id { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }

        public string CustomerName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public bool Status { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }

        public int VoucherId { get; set; }

        public Voucher Voucher { get; set; }

        public string UserId { get; set; }
    }
}
