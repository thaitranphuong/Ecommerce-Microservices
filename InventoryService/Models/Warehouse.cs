using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InventoryService.Models
{
    public class Warehouse
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public ICollection<Import> Imports { get; set; }
    }
}
