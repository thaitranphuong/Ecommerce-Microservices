namespace InventoryService.Dtos
{
    public class ProductOfWarehouseDto
    {
        public int Id { get; set; }

        public int WarehouseQuantity { get; set; }

        public float Price { get; set; }

        public string Name { get; set; }

        public string Thumbnail { get; set; }

        public string Unit { get; set; }
    }
}
