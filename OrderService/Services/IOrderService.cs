using OrderService.Constants;
using OrderService.Dtos;
using OrderService.Dtos.Paginations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderService.Services
{
    public interface IOrderService
    {
        Task<bool> Save(OrderDto dto);
        Task<OrderDto> FindById(int id);
        Task<OrderOutput> FindAll(OrderStatus status, int page, int limit);
        Task<List<OrderDto>> FindAllByUserId(string userId, OrderStatus status);
        Task<List<OrderDto>> FindAllByYear(int year);
    }
}
