using AutoMapper;
using ProductService.Dtos;
using ProductService.Models;
using ProductService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Services.Implements
{
    public class ProductDetailService : IProductDetailService
    {
        private readonly IProductDetailRepository _productDetailRepository;
        private readonly IMapper _mapper;

        public ProductDetailService(IProductDetailRepository productDetailRepository, IMapper mapper)
        {
            _productDetailRepository = productDetailRepository;
            _mapper = mapper;
        }

        public async Task<bool> DeleteById(int id)
        {
            var productDetail = await _productDetailRepository.FindById(id);
            return await _productDetailRepository.Remove(productDetail) > 0;
        }

        public async Task<bool> Save(ProductDetailDto dto)
        {
            var result = await _productDetailRepository.CreateOne(_mapper.Map<ProductDetail>(dto)) > 0;
            return result;
        }
    }
}
