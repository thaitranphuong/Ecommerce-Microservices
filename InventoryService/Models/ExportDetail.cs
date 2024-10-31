using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryService.Models
{
    public class ExportDetail
    {
        public int Id { get; set; }

        public int Quantity { get; set; }

        public float Price { get; set; }

        [Required]
        public int ProductId { get; set; }

        public int ExportId { get; set; }
        public Export Export { get; set; }
    }
}
