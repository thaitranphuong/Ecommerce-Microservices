using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.MVC.Models.Pagination
{
    public class ProductOutput : AbtractOutput<ProductViewModel>
    {
        public int CategoryId { get; set; }
        public float Price { get; set; }
    }
}
