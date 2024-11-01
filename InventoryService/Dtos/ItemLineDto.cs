namespace InventoryService.Dtos
{
    public class ItemLineDto
    {
        public int Id { get; set; }
        public int ImportDetailId { get; set; }
        public string Name { get; set; }
        public int ImportId { get; set; }
        public int ProductId { get; set; }
        public bool IsExpired { get; set; }
        public int RemainingQuantity { get; set; }
        public string Unit { get; set; }
    }
}
