using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InventoryService.Models
{
    public class Export
    {
        public int Id { get; set; }

        public DateTime CreatedTime { get; set; }

        public string Reason { get; set; }

        public bool IsExpired { get; set; }

        public string ReceiverName { get; set; }

        [Required]
        public string UserId { get; set; }

        public ICollection<ExportDetail> ExportDetails { get; set; }

        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }

    }
}
