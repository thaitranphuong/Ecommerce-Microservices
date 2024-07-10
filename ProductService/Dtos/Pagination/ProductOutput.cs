﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Dtos.Pagination
{
    public class ProductOutput : AbtractOutput<ProductDto>
    {
        public int CategoryId { get; set; }
        public float Price { get; set; }
    }
}
