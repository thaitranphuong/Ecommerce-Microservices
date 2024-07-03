using AutoMapper;
using ProductService.Dtos;
using ProductService.Dtos.Pagination;
using ProductService.Models;
using ProductService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Services.Implements
{
    public class ProductServices : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductServices(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<int> Save(ProductDto dto)
        {
            var result = 0;
            if (dto.Id == 0)
            {
                result = await _productRepository.CreateOne(_mapper.Map<Product>(dto));
            }
            else
            {
                Product product = await _productRepository.FindById(dto.Id);
                _mapper.Map(dto, product);
                result = await _productRepository.SaveChange();
            }
            return result;
        }

        public async Task<ProductDto> FindById(int id)
        {
            return _mapper.Map<ProductDto>(await _productRepository.FindById(id));
        }

        public async Task<ProductOutput> FindAll(string name, int page, int limit)
        {
            var products = await _productRepository.FindAll(name, page, limit);
            var productDtos = new List<ProductDto>();
            foreach (var product in products)
            {
                productDtos.Add(_mapper.Map<ProductDto>(product));
            }
            ProductOutput output = new ProductOutput();
            output.Name = name;
            output.Page = page;
            output.TotalPage = (int) Math.Ceiling((double)(await _productRepository.FindByName(name)).Count / limit);
            output.ListResult = productDtos;
            return output;
        }
    }
}
