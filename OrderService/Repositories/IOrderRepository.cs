using Microsoft.EntityFrameworkCore;
using OrderService.Constants;
using OrderService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Repositories
{
    public interface IOrderRepository
    {
        Task<int> CreateOne(Order order);
        Task<Order> FindById(int id);
        Task<List<Order>> FindByStatus(OrderStatus status);
        Task<List<Order>> FindAll(int page, int limit, OrderStatus status);
        Task<List<Order>> FindAllByYear(int year);
        Task<List<Order>> FindByUserIdAndStatus(string userId, OrderStatus status);
        Task<int> SaveChange();
        Task<Models.OrderDetail> FindOrderDetail(int orderId, int productId);
        Task<List<Order>> FindAllByYearToStatistic(int year);

        Task<List<Order>> FindAllByMonthToStatistic(int month, int year);

        Task<List<Order>> FindAllByDateToStatistic(DateTime startDate, DateTime endDate);
    }
}
