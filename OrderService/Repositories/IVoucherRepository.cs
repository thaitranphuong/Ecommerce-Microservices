using OrderService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Repositories
{
    public interface IVoucherRepository
    {
        Task<int> CreateOne(Voucher voucher);
        Task<Voucher> FindById(int id);
        Task<List<Voucher>> FindByName(string name);
        Task<List<Voucher>> FindAll(string name, int page, int limit);
        Task<List<Voucher>> FindByNameOfCustomerPage(string name);
        Task<List<Voucher>> FindAllOfCustomerPage(string name, int page, int limit);
        Task<int> SaveChange();
        Task<int> Remove(int id);
    }
}
