using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InventoryService.Models
{
    public class Import
    {
        public int Id { get; set; }

        public DateTime CreatedTime { get; set; }

        [Required]
        public string UserId { get; set; }

        public ICollection<ImportDetail> ImportDetails { get; set; }

        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }

        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }

    }
}
