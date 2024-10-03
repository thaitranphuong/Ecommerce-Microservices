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
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;

        public SupplierService(ISupplierRepository supplierRepository, IMapper mapper)
        {
            _supplierRepository = supplierRepository;
            _mapper = mapper;
        }

        public async Task<bool> Save(SupplierDto dto)
        {
            var result = false;
            if (dto.Id == 0)
            {
                result = await _supplierRepository.CreateOne(_mapper.Map<Supplier>(dto)) > 0;
            }
            else
            {
                Supplier supplier = await _supplierRepository.FindById(dto.Id);
                supplier.Name = dto.Name;
                supplier.Address = dto.Address;
                supplier.Phone = dto.Phone;
                supplier.Email = dto.Email;
                result = await _supplierRepository.SaveChange() > 0;
            }
            return result;
        }

        public async Task<SupplierDto> FindById(int id)
        {
            return _mapper.Map<SupplierDto>(await _supplierRepository.FindById(id));
        }

        public async Task<SupplierOutput> FindAll(string name, int page, int limit)
        {
            var suppliers = await _supplierRepository.FindAll(name, page, limit);
            var SupplierDtos = new List<SupplierDto>();
            foreach (var supplier in suppliers)
            {
                SupplierDtos.Add(_mapper.Map<SupplierDto>(supplier));
            }
            SupplierOutput output = new SupplierOutput();
            output.Name = name;
            output.Page = page;
            output.TotalPage = (int)Math.Ceiling((double)(await _supplierRepository.FindByName(name)).Count / limit);
            output.ListResult = SupplierDtos;
            return output;
        }

        public async Task<bool> DeleteById(int id)
        {
            var supplier = await _supplierRepository.FindById(id);
            return await _supplierRepository.Remove(supplier) > 0;
        }
    }
}
