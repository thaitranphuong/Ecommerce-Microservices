using AutoMapper;
using ProductService.Dtos;
using ProductService.Dtos.Pagination;
using ProductService.Models;
using ProductService.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductService.Services.Implements
{
    public class UnitService : IUnitService
    {
        private readonly IUnitRepository _unitRepository;
        private readonly IMapper _mapper;

        public UnitService(IUnitRepository unitRepository, IMapper mapper)
        {
            _unitRepository = unitRepository;
            _mapper = mapper;
        }

        public async Task<bool> Save(UnitDto dto)
        {
            var result = false;
            if (dto.Id == 0)
            {
                result = await _unitRepository.CreateOne(_mapper.Map<Unit>(dto)) > 0;
            }
            else
            {
                Unit unit = await _unitRepository.FindById(dto.Id);
                unit.Name = dto.Name;
                result = await _unitRepository.SaveChange() > 0;
            }
            return result;
        }

        public async Task<UnitDto> FindById(int id)
        {
            return _mapper.Map<UnitDto>(await _unitRepository.FindById(id));
        }

        public async Task<UnitOutput> FindAll(string name, int page, int limit)
        {
            var units = await _unitRepository.FindAll(name, page, limit);
            var unitDtos = new List<UnitDto>();
            foreach (var unit in units)
            {
                unitDtos.Add(_mapper.Map<UnitDto>(unit));
            }
            UnitOutput output = new UnitOutput();
            output.Name = name;
            output.Page = page;
            output.TotalPage = (int) Math.Ceiling((double)(await _unitRepository.FindByName(name)).Count / limit);
            output.ListResult = unitDtos;
            return output;
        }

        public async Task<bool> DeleteById(int id)
        {
            var category = await _unitRepository.FindById(id);
            if (category.Products.Count > 0)
                return false;
            return await _unitRepository.Remove(category) > 0;
        }
    }
}
