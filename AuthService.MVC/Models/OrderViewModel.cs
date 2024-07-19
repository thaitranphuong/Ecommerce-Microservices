using AuthService.MVC.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.MVC.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }

        public string CustomerName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public OrderStatus Status { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public int VoucherId { get; set; }

        public string VoucherName { get; set; }

        public float VoucherDiscountPercent { get; set; }

        public string UserId { get; set; }

        public ICollection<OrderDetailViewModel> OrderDetails { get; set; }
    }
}
