using ProductService.Models;
using System.Threading.Tasks;

namespace ProductService.Repositories
{
    public interface IUserRepository
    {
        Task<int> CreateOne(User user);
        Task<User> FindById(string id);
        Task<int> SaveChange();
    }
}
