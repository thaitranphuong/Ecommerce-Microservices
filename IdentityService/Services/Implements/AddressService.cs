using AutoMapper;
using IdentityService.Models;
using IdentityService.Models.DTOs;
using IdentityService.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityService.Services.Implements
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;

        public AddressService(IAddressRepository userRepository, IMapper mapper)
        {
            _addressRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<bool> Create(AddressDto addressDto)
        {
            return await _addressRepository.CreateOne(_mapper.Map<Address>(addressDto)) > 0;
        }

        public async Task<bool> Update(AddressDto addressDto)
        {
            var address = await _addressRepository.FindById(addressDto.Id);
            if (address == null) return false;
            address.Name = addressDto.Name;
            address.Phone = addressDto.Phone;
            address.City = addressDto.City;
            address.District = addressDto.District;
            address.Ward = addressDto.Ward;
            address.Street = addressDto.Street;
            return await _addressRepository.SaveChange() > 0;
        }

        public async Task<List<AddressDto>> FindAllByUserId(string userId)
        {
            var addresses = await _addressRepository.FindAllByUserId(userId);
            var addressDtos = new List<AddressDto>();
            foreach (var address in addresses)
            {
                addressDtos.Add(_mapper.Map<AddressDto>(address));
            }
            return addressDtos;
        }

        public async Task<AddressDto> FindById(int id)
        {
            return _mapper.Map<AddressDto>(await _addressRepository.FindById(id));
        }

        public async Task<bool> Remove(int id)
        {
            return await _addressRepository.Remove(id) > 0;
        }
    }
}
