namespace InventoryService.Dtos
{
    public class InStockProductDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Thumbnail { get; set; }

        public string Unit { get; set; }

        public int RemaningQuantity { get; set; }

        public int ExpiredQuantity { get; set; }

        public int ExportedQuantity { get; set; }

    }
}
