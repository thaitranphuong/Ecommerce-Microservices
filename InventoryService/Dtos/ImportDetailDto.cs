using InventoryService.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace InventoryService.Dtos
{
    public class ImportDetailDto
    {
        public int Id { get; set; }

        public int Quantity { get; set; }

        public float Price { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductThumbnail {  get; set; }

        public string Unit { get; set; }

        public int ImportId { get; set; }

    }
}
