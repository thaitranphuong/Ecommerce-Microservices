using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Models
{
    [Table("Vouchers")]
    public class Voucher
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public float DiscountPercent { get; set; }

        public int Quantity { get; set; }

        public float MaxDiscount { get; set; }

        public int UsedQuantity { get; set; }

        public bool IsRemoved { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public ICollection<Order> Orders { get; set; }
    } 
}
