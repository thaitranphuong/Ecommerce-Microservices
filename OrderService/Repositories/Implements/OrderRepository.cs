

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
            if (status != OrderStatus.ALL)
                return await _context.Orders.Include(o => o.OrderDetails)
                   .Include(o => o.Voucher)
                   .Where(o => o.Status == status)
                   .OrderByDescending(o => o.CreatedTime)
                   .Skip((page - 1) * limit)
                   .Take(limit)
                   .ToListAsync();

            return await _context.Orders.Include(o => o.OrderDetails)
                    .Include(o => o.Voucher)
                    .OrderByDescending(o => o.CreatedTime)
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .ToListAsync();
        }

        public async Task<List<Order>> FindAllByYear(int year)
        {
            return await _context.Orders
                    .Include(o => o.OrderDetails)
                    .Include(o => o.Voucher)
                    .Where(o => o.CreatedTime.Year == year)
                    .ToListAsync();
        }

        public async Task<Order> FindById(int id)
        {
            return await _context.Orders.Include(o => o.OrderDetails).Include(o => o.Voucher).FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<Order>> FindByStatus(OrderStatus status = OrderStatus.ALL)
        {
            if (status == OrderStatus.ALL)
                return await _context.Orders.Include(o => o.OrderDetails).Include(o => o.Voucher)
                    .OrderBy(o => o.CreatedTime)
                    .ToListAsync();

            return await _context.Orders.Include(o => o.OrderDetails).Include(o => o.Voucher)
                    .OrderBy(o => o.CreatedTime)
                    .Where(o => o.Status == status)
                    .ToListAsync();
        }

        public async Task<List<Order>> FindByUserIdAndStatus(string userId, OrderStatus status = OrderStatus.ALL)
        {
            if (status == OrderStatus.ALL)
                return await _context.Orders.Include(o => o.OrderDetails).Include(o => o.Voucher)
                    .Where(o => o.UserId == userId)
                    .OrderByDescending(o => o.CreatedTime)
                    .ToListAsync();

            return await _context.Orders.Include(o => o.OrderDetails).Include(o => o.Voucher)
                    .Where(o => o.Status == status && o.UserId == userId)
                    .OrderByDescending(o => o.CreatedTime)
                    .ToListAsync();
        }

        public async Task<Models.OrderDetail> FindOrderDetail(int orderId, int productId)
        {
            var orderDetail = await _context.OrderDetails.FirstOrDefaultAsync(o => o.ProductId == productId && o.OrderId == orderId);
            return orderDetail;
        }

        public async Task<int> SaveChange()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
