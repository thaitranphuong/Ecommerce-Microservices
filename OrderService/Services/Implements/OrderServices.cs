using AutoMapper;
using OrderService.Constants;
using OrderService.Dtos;
using OrderService.Dtos.Paginations;
using OrderService.Models;
using OrderService.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderService.Services.Implements
{
    public class OrderServices : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderServices(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<OrderOutput> FindAll(OrderStatus status, int page, int limit)
        {
            var orders = await _orderRepository.FindAll(page, limit, status);
            var orderDtos = new List<OrderDto>();
            foreach (var order in orders)
            {
                var orderDto = _mapper.Map<OrderDto>(order);
                var orderDetailDtos = new List<OrderDetailDto>();
                foreach(var orderDetail in order.OrderDetails)
                {
                    orderDetailDtos.Add(_mapper.Map<OrderDetailDto>(orderDetail));
                }
                orderDto.OrderDetails = orderDetailDtos;
                orderDtos.Add(orderDto);
            }
            OrderOutput output = new OrderOutput();
            output.Status = status;
            output.Page = page;
            output.TotalPage = (int)Math.Ceiling((double)(await _orderRepository.FindByStatus(status)).Count / limit);
            output.ListResult = orderDtos;
            return output;
        }

        public async Task<List<OrderDto>> FindAllByUserId(string userId, OrderStatus status)
        {
            var orders = await _orderRepository.FindByUserIdAndStatus(userId, status);
            var orderDtos = new List<OrderDto>();
            foreach (var order in orders)
            {
                var orderDto = _mapper.Map<OrderDto>(order);
                var orderDetailDtos = new List<OrderDetailDto>();
                foreach (var orderDetail in order.OrderDetails)
                {
                    orderDetailDtos.Add(_mapper.Map<OrderDetailDto>(orderDetail));
                }
                orderDto.OrderDetails = orderDetailDtos;
                orderDtos.Add(orderDto);
            }
            return orderDtos;
        }

        public async Task<OrderDto> FindById(int id)
        {
            return _mapper.Map<OrderDto>(await _orderRepository.FindById(id));
        }

        public async Task<bool> Save(OrderDto dto)
        {
            var result = false;
            if (dto.Id == 0)
            {
                var order = _mapper.Map<Order>(dto);
                if (order.VoucherId == 0) order.VoucherId = null;
                var orderDetails = new List<OrderDetail>();
                foreach(var detail in dto.OrderDetails)
                {
                    orderDetails.Add(_mapper.Map<OrderDetail>(detail));
                }
                return await _orderRepository.CreateOne(order) > 0;
            }
            else
            {
                Order order = await _orderRepository.FindById(dto.Id);
                order.Status = dto.Status;
                result = await _orderRepository.SaveChange() > 0;
            }
            return result;
        }
    }
}
