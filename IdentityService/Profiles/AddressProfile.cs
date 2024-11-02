using AutoMapper;
using IdentityService.Models;
using IdentityService.Models.DTOs;

namespace IdentityService.Profiles
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<AddressDto, Address>();
            CreateMap<Address, AddressDto>();
        }
    }
}
