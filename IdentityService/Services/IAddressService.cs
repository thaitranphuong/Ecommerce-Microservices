using IdentityService.Models;
using IdentityService.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityService.Services
{
    public interface IAddressService
    {
        Task<bool> Create(AddressDto addressDto);
        Task<bool> Update(AddressDto addressDto);
        Task<List<AddressDto>> FindAllByUserId(string userId);
        Task<AddressDto> FindById(int id);
        Task<bool> Remove(int id);
    }
}
