using IdentityService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityService.Repositories
{
    public interface IUserRepository
    {
        Task<bool> CreateOne(User user);
        Task<User> FindByEmail(string email);
        Task<List<User>> FindAll(string email, int page, int limit);
        Task<List<User>> FindAllNoPagination(string email);
        Task<List<User>> FindAllCustomer();
        Task<List<User>> FindAllAdmin();
    }
}
