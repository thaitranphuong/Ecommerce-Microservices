using IdentityService.Models.Dtos.Pagination;
using IdentityService.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityService.Services
{
    public interface IUserService
    {
        Task<UserDto> Create(UserDto user);
        Task<UserDto> GoogleLogin(string name, string email, string avatar);
        Task<UserDto> Update(UserDto user);
        Task<bool> ChangePassword(string id, string oldPassword, string newPassword);
        Task<UserDto> FindByEmail(string email);
        Task<UserDto> FindById(string id);
        Task<UserDto> Login(string email, string password);
        Task<UserOutput> FindAll(string email, int page, int limit);
        Task<List<UserDto>> FindAllNoPagination();
        Task<List<UserDto>> FindAllCustomer();
        Task<List<UserDto>> FindAllAdmin();
        Task<int> SaveShowHide(string id);
        Task<int> ActiveAccount(string id);
    }
}
