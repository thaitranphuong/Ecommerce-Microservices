using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Dtos.Pagination
{
    public class CategoryOutput : AbtractOutput<CategoryDto>
    {
        public string Name { get; set; }
    }
}
