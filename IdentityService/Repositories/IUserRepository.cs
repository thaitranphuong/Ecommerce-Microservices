using IdentityService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityService.Repositories
{
    public interface IUserRepository
    {
        Task<bool> CreateOne(User user, bool isAdmin);
        Task<User> FindByEmail(string email);
        Task<List<User>> FindAll(string email, int page, int limit);
        Task<List<User>> FindAllNoPagination(string email);
        Task<List<User>> FindAllCustomer();
        Task<List<User>> FindAllAdmin();
        Task<User> FindById(string id);
        Task<int> SaveChange();
        Task<Role> FindRoleByName(string name);
    }
}
