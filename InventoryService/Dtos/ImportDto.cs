using InventoryService.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace InventoryService.Dtos
{
    public class ImportDto
    {
        public int Id { get; set; }

        public DateTime CreatedTime { get; set; }
        
        public string UserId { get; set; }

        public string UserName { get; set; }

        public int WarehouseId { get; set; }

        public string WarehouseName { get; set; }
        
        public int SupplierId { get; set; }

        public string SupplierName { get; set;}

        public List<ImportDetailDto> ImportDetails { get; set; }
        
    }
}
