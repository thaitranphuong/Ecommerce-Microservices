using IdentityService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityService.Repositories
{
    public interface IAddressRepository
    {
        Task<int> CreateOne(Address address);
        Task<List<Address>> FindAllByUserId(string userId);
        Task<Address> FindById(int id);
        Task<int> SaveChange();
        Task<int> Remove(int id);
    }
}
