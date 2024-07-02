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
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<bool> Save(CategoryDto dto)
        {
            var result = false;
            if (dto.Id == 0)
            {
                result = await _categoryRepository.CreateOne(_mapper.Map<Category>(dto)) > 0;
            }
            else
            {
                Category category = await _categoryRepository.FindById(dto.Id);
                _mapper.Map(dto, category);
                result = await _categoryRepository.SaveChange() > 0;
            }
            return result;
        }

        public async Task<CategoryDto> FindById(int id)
        {
            return _mapper.Map<CategoryDto>(await _categoryRepository.FindById(id));
        }

        public async Task<CategoryOutput> FindAll(string name, int page, int limit)
        {
            var categories = await _categoryRepository.FindAll(name, page, limit);
            var categoryDtos = new List<CategoryDto>();
            foreach (var category in categories)
            {
                categoryDtos.Add(_mapper.Map<CategoryDto>(category));
            }
            CategoryOutput output = new CategoryOutput();
            output.Name = name;
            output.Page = page;
            output.TotalPage = (int) Math.Ceiling((double)(await _categoryRepository.FindByName(name)).Count / limit);
            output.ListResult = categoryDtos;
            return output;
        }

        public async Task<bool> DeleteById(int id)
        {
            var category = await _categoryRepository.FindById(id);
            return await _categoryRepository.Remove(category) > 0;
        }
    }
}
