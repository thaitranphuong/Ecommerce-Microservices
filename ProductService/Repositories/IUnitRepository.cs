using ProductService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductService.Repositories
{
    public interface IUnitRepository
    {
        Task<int> CreateOne(Unit unit);
        Task<Unit> FindById(int id);
        Task<List<Unit>> FindByName(string name);
        Task<List<Unit>> FindAll(string name, int page, int limit);
        Task<int> SaveChange();
        Task<int> Remove(Unit unit);
    }
}
