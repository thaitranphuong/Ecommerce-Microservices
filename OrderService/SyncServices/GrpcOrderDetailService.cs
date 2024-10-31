using Grpc.Core;
using System.Threading.Tasks;
using OrderService.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;

namespace OrderService.SyncServices
{
    public class GrpcOrderDetailService : GrpcOrderDetail.GrpcOrderDetailBase
    {
        private readonly AppDbContext _context;

        public GrpcOrderDetailService(AppDbContext context)
        {
            _context = context;
        }

        public async override Task<OrderDetailResponse> GetOrderDetail(OrderDetailRequest request, ServerCallContext context)
        {
            var orderId = request.OrderId;
            var productId = request.ProductId;
            var response = new OrderDetailResponse();

            if (productId != 0 && orderId == 0)
            {
                var orderDetails = await _context.OrderDetails.Where(o => o.ProductId == productId).ToListAsync();
                if(orderDetails.Any())
                {
                    foreach (var orderDetail in orderDetails)
                    {
                        response.OrderDetails.Add(new OrderDetail()
                        {
                            OrderId = orderDetail.OrderId,
                            ProductId = orderDetail.ProductId,
                            Price = orderDetail.Price,
                            Quantity = orderDetail.Quantity,
                        });
                    }
                }
                return response;
            }

            return response;
        }
    }
}
