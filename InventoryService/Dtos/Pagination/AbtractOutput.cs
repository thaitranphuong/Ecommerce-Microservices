using System.Collections.Generic;

namespace InventoryService.Dtos.Pagination
{
    public class AbtractOutput<T>
    {
        public int Page { get; set; }
        public int TotalPage { get; set; }
        public string Name { get; set; }
        public List<T> ListResult { get; set; } = new List<T>();
    }
}
