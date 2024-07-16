using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Dtos
{
    public class VoucherDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public float DiscountPercent { get; set; }

        public int Quantity { get; set; }

        public int UsedQuantity { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
