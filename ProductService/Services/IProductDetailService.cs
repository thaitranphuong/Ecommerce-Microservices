using ProductService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Services
{
    public interface IProductDetailService
    {
        Task<bool> Save(ProductDetailDto productDetail);
        Task<bool> DeleteById(int id);
    }
}
