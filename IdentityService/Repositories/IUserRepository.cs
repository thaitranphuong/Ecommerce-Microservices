using IdentityService.Models;
using System.Threading.Tasks;

namespace IdentityService.Repositories
{
    public interface IUserRepository
    {
        Task<bool> CreateOne(User user);
        Task<User> FindByEmail(string email);
    }
}
