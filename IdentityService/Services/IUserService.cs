using IdentityService.Models.DTOs;
using System.Threading.Tasks;

namespace IdentityService.Services
{
    public interface IUserService
    {
        Task<UserDto> Create(UserDto user);
        Task<UserDto> FindByEmail(string email);
        Task<UserDto> Login(string email, string password);
    }
}
