using AutoMapper;
using OrderService.Dtos;
using OrderService.Dtos.Paginations;
using OrderService.Models;
using OrderService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Services.Implements
{
    public class VoucherService : IVoucherService
    {
        private readonly IVoucherRepository _voucherRepository;
        private readonly IMapper _mapper;

        public VoucherService(IVoucherRepository voucherRepository, IMapper mapper)
        {
            _voucherRepository = voucherRepository;
            _mapper = mapper;
        }

        public async Task<bool> DeleteById(int id)
        {
            return await _voucherRepository.Remove(id) > 0;
        }

        public async Task<VoucherOutput> FindAll(string name, int page, int limit)
        {
            var vouchers = await _voucherRepository.FindAll(name, page, limit);
            var voucherDtos = new List<VoucherDto>();
            foreach (var category in vouchers)
            {
                voucherDtos.Add(_mapper.Map<VoucherDto>(category));
            }
            VoucherOutput output = new VoucherOutput();
            output.Name = name;
            output.Page = page;
            output.TotalPage = (int)Math.Ceiling((double)(await _voucherRepository.FindByName(name)).Count / limit);
            output.ListResult = voucherDtos;
            return output;
        }

        public async Task<VoucherOutput> FindAllOfCustomerPage(string name, int page, int limit)
        {
            var vouchers = await _voucherRepository.FindAllOfCustomerPage(name, page, limit);
            var voucherDtos = new List<VoucherDto>();
            foreach (var category in vouchers)
            {
                voucherDtos.Add(_mapper.Map<VoucherDto>(category));
            }
            VoucherOutput output = new VoucherOutput();
            output.Name = name;
            output.Page = page;
            output.TotalPage = (int)Math.Ceiling((double)(await _voucherRepository.FindByNameOfCustomerPage(name)).Count / limit);
            output.ListResult = voucherDtos;
            return output;
        }

        public async Task<VoucherDto> FindById(int id)
        {
            return _mapper.Map<VoucherDto>(await _voucherRepository.FindById(id));
        }

        public async Task<bool> Save(VoucherDto dto)
        {
            var result = false;
            if (dto.Id == 0)
            {
                result = await _voucherRepository.CreateOne(_mapper.Map<Voucher>(dto)) > 0;
            }
            else
            {
                Voucher voucher = await _voucherRepository.FindById(dto.Id);
                _mapper.Map(dto, voucher);
                result = await _voucherRepository.SaveChange() > 0;
            }
            return result;
        }
    }
}
