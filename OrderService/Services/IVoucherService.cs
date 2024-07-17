using OrderService.Dtos;
using OrderService.Dtos.Paginations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Services
{
    public interface IVoucherService
    {
        Task<bool> Save(VoucherDto dto);
        Task<VoucherDto> FindById(int id);
        Task<VoucherOutput> FindAll(string name, int page, int limit);
        Task<VoucherOutput> FindAllOfCustomerPage(string name, int page, int limit);
        Task<bool> DeleteById(int id);
    }
}
