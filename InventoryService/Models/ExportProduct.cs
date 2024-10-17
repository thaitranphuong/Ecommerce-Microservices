namespace InventoryService.Models
{
    public class ExportProduct
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }

    }
}
