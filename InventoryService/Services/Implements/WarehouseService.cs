using AutoMapper;
using InventoryService.Dtos;
using InventoryService.Dtos.Pagination;
using InventoryService.Models;
using InventoryService.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryService.Services.Implements
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IMapper _mapper;

        public WarehouseService(IWarehouseRepository warehouseRepository, IMapper mapper)
        {
            _warehouseRepository = warehouseRepository;
            _mapper = mapper;
        }

        public async Task<bool> Save(WarehouseDto dto)
        {
            var result = false;
            if (dto.Id == 0)
            {
                result = await _warehouseRepository.CreateOne(_mapper.Map<Warehouse>(dto)) > 0;
            }
            else
            {
                Warehouse warehouse = await _warehouseRepository.FindById(dto.Id);
                warehouse.Name = dto.Name;
                warehouse.Address = dto.Address;
                result = await _warehouseRepository.SaveChange() > 0;
            }
            return result;
        }

        public async Task<WarehouseDto> FindById(int id)
        {
            return _mapper.Map<WarehouseDto>(await _warehouseRepository.FindById(id));
        }

        public async Task<WarehouseOutput> FindAll(string name, int page, int limit)
        {
            var warehouses = await _warehouseRepository.FindAll(name, page, limit);
            var warehouseDtos = new List<WarehouseDto>();
            foreach (var warehouse in warehouses)
            {
                warehouseDtos.Add(_mapper.Map<WarehouseDto>(warehouse));
            }
            WarehouseOutput output = new WarehouseOutput();
            output.Name = name;
            output.Page = page;
            output.TotalPage = (int) Math.Ceiling((double)(await _warehouseRepository.FindByName(name)).Count / limit);
            output.ListResult = warehouseDtos;
            return output;
        }

        public async Task<bool> DeleteById(int id)
        {
            var warehouse = await _warehouseRepository.FindById(id);
            return await _warehouseRepository.Remove(warehouse) > 0;
        }
    }
}
