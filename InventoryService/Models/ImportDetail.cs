using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryService.Models
{
    public class ImportDetail
    {
        public int Id { get; set; }

        public int Quantity { get; set; }

        public float Price { get; set; }

        public int ExportedQuantity { get; set; }

        public string Position { get; set; }

        [Required]
        public int ProductId { get; set; }

        public int ImportId { get; set; }
        public Import Import { get; set; }
    }
}
