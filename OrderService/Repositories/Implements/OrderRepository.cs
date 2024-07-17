

using Microsoft.EntityFrameworkCore;
using OrderService.Constants;
using OrderService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Repositories.Implements
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateOne(Order order)
        {
            await _context.Orders.AddAsync(order);
            int count = await _context.SaveChangesAsync();
            return count;
        }

        public async Task<List<Order>> FindAll(int page, int limit, OrderStatus status = OrderStatus.ALL)
        {
            return await _context.Orders
                    .Where(o => o.Status == status)
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .ToListAsync();

        }

        public async Task<Order> FindById(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<List<Order>> FindByStatus(OrderStatus status)
        {
            return await _context.Orders
                    .Where(o => o.Status == status)
                    .ToListAsync();
        }

        public async Task<List<Order>> FindByUserIdAndStatus(string userId, OrderStatus status = OrderStatus.ALL)
        {
            return await _context.Orders
                    .Where(o => o.Status == status && o.UserId == userId)
                    .ToListAsync();
        }

        public async Task<int> SaveChange()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
