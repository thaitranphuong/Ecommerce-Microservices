using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Dtos.Pagination
{
    public class AbstractOutput<T>
    {
        public int Page { get; set; }
        public int TotalPage { get; set; }
        public string Name { get; set; }
        public List<T> ListResult { get; set; } = new List<T>();
    }
}
