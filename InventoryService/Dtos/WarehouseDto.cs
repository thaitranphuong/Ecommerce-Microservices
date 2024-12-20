﻿using System.Collections.Generic;

namespace InventoryService.Dtos
{
    public class WarehouseDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public List<InStockProductDto> InStockProducts { get; set; }
    }
}
