using System.Collections.Generic;
using System;

namespace InventoryService.Dtos
{
    public class ExportDto
    {
        public int Id { get; set; }

        public DateTime CreatedTime { get; set; }

        public string Reason { get; set; }

        public string ReceiverName { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public int WarehouseId { get; set; }

        public string WarehouseName { get; set; }

        public List<ExportDetailDto> ExportDetails { get; set; }
        
    }
}
