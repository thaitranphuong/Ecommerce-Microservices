using IdentityService.Models.Dtos.Pagination;
using IdentityService.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityService.Services
{
    public interface IUserService
    {
        Task<UserDto> Create(UserDto user);
        Task<UserDto> FindByEmail(string email);
        Task<UserDto> Login(string email, string password);
        Task<UserOutput> FindAll(string email, int page, int limit);
        Task<List<UserDto>> FindAllNoPagination();
        Task<List<UserDto>> FindAllCustomer();
        Task<List<UserDto>> FindAllAdmin();
    }
}
