using ProductService.Dtos;
using ProductService.Dtos.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Services
{
    public interface IUnitService
    {
        Task<bool> Save(UnitDto unit);
        Task<UnitDto> FindById(int id);
        Task<UnitOutput> FindAll(string name, int page, int limit);
        Task<bool> DeleteById(int id);
    }
}
