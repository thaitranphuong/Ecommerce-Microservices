using AutoMapper;
using ProductService.Dtos;
using ProductService.Dtos.Pagination;
using ProductService.Models;
using ProductService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

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
                product.Name = dto.Name;
                product.ShortDescription = dto.ShortDescription;
                product.FullDescription = dto.FullDescription;
                product.Price = dto.Price;
                product.Thumbnail = dto.Thumbnail;
                product.DiscountPercent = dto.DiscountPercent;
                product.CategoryId = dto.CategoryId;
                product.Expiry = dto.Expiry;
                product.Origin = dto.Origin;
                product.UnitId = dto.UnitId;
                result = await _productRepository.SaveChange();
            }
            return result;
        }

        public async Task<int> SaveShowHide(int id)
        {
            Product product = await _productRepository.FindById(id);
            product.Enabled = !product.Enabled;
            return await _productRepository.SaveChange();
        }

        public async Task<ProductDto> FindById(int id)
        {
            var product = await _productRepository.FindById(id);
            var productDto = _mapper.Map<ProductDto>(product);
            return productDto;
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

        public async Task<ProductOutput> FindAll(string name, int categoryId, int unitId, float price, int page, int limit)
        {
            if (float.IsNaN(price)) price = 0;
            var products = await _productRepository.FindAll(name, categoryId, unitId, price, page, limit);
            var productDtos = new List<ProductDto>();
            foreach (var product in products)
            {
                productDtos.Add(_mapper.Map<ProductDto>(product));
            }
            ProductOutput output = new ProductOutput();
            output.Name = name;
            output.Page = page;
            output.Price = price;
            output.CategoryId = categoryId;
            output.TotalPage = (int)Math.Ceiling((double)(await _productRepository.FindByNameAndCategoryIdAndPrice(name, categoryId, unitId, price)).Count / limit);
            output.ListResult = productDtos;
            return output;
        }

        public async Task<ProductOutput> FindAllByOrderBySoldquantity(int page, int limit)
        {
            var products = await _productRepository.FindAllByOrderBySoldquantity(page, limit);
            var productDtos = new List<ProductDto>();
            foreach (var product in products)
            {
                productDtos.Add(_mapper.Map<ProductDto>(product));
            }
            ProductOutput output = new ProductOutput();
            output.ListResult = productDtos;
            return output;
        }
    }
}
