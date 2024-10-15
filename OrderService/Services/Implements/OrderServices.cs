using AutoMapper;
using OrderService.AsyncServices;
using OrderService.Constants;
using OrderService.Dtos;
using OrderService.Dtos.Paginations;
using OrderService.Models;
using OrderService.Repositories;
using OrderService.SyncServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderService.Services.Implements
{
    public class OrderServices : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IVoucherService _voucherService;
        private readonly IMapper _mapper;
        private readonly IGrpcProductService _grpcProductService;
        private readonly IGrpcUserService _grpcUserService;
        private readonly IMessageProducer _messageProducer;

        public OrderServices(IOrderRepository orderRepository, IMapper mapper, IGrpcProductService grpcProductService, IMessageProducer messageProducer, IVoucherService voucherService, IGrpcUserService grpcUserService)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _grpcProductService = grpcProductService;
            _messageProducer = messageProducer;
            _voucherService = voucherService;
            _grpcUserService = grpcUserService;
        }

        public async Task<OrderOutput> FindAll(OrderStatus status, int page, int limit)
        {
            var orders = await _orderRepository.FindAll(page, limit, status);
            var orderDtos = new List<OrderDto>();
            foreach (var order in orders)
            {
                var orderDto = _mapper.Map<OrderDto>(order);
                if (order.VoucherId != null)
                {
                    orderDto.VoucherDiscountPercent = order.Voucher.DiscountPercent;
                    orderDto.VoucherName = order.Voucher.Name;
                    orderDto.VoucherMaxDiscount = order.Voucher.MaxDiscount;
                }
                var orderDetailDtos = new List<OrderDetailDto>();
                foreach(var orderDetail in order.OrderDetails)
                {
                    var orderDetailDto = _mapper.Map<OrderDetailDto>(orderDetail);
                    ProductResponse product = await _grpcProductService.GetProduct(orderDetail.ProductId);
                    orderDetailDto.Thumbnail = product.Thumbnail;
                    orderDetailDto.Name = product.Name;
                    orderDetailDto.Price = product.Price;
                    orderDetailDto.Unit = product.Unit;
                    orderDetailDtos.Add(orderDetailDto);
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
                if(order.VoucherId != null)
                {
                    orderDto.VoucherDiscountPercent = order.Voucher.DiscountPercent;
                    orderDto.VoucherName = order.Voucher.Name;
                    orderDto.VoucherMaxDiscount = order.Voucher.MaxDiscount;
                }
                var orderDetailDtos = new List<OrderDetailDto>();
                foreach (var orderDetail in order.OrderDetails)
                {
                    var orderDetailDto = _mapper.Map<OrderDetailDto>(orderDetail);
                    ProductResponse product = await _grpcProductService.GetProduct(orderDetail.ProductId);
                    orderDetailDto.Thumbnail = product.Thumbnail;
                    orderDetailDto.Name = product.Name;
                    orderDetailDto.Price = product.Price;
                    orderDetailDto.Unit = product.Unit;
                    orderDetailDtos.Add(orderDetailDto);
                }
                orderDto.OrderDetails = orderDetailDtos;
                orderDtos.Add(orderDto);
            }
            return orderDtos;
        }

        public async Task<List<OrderDto>> FindAllByYear(int year)
        {
            var orders = await _orderRepository.FindAllByYear(year);
            var orderDtos = new List<OrderDto>();
            foreach (var order in orders)
            {
                var orderDto = _mapper.Map<OrderDto>(order);
                if (order.VoucherId != null)
                {
                    orderDto.VoucherDiscountPercent = order.Voucher.DiscountPercent;
                    orderDto.VoucherName = order.Voucher.Name;
                    orderDto.VoucherMaxDiscount = order.Voucher.MaxDiscount;
                }
                var orderDetailDtos = new List<OrderDetailDto>();
                foreach (var orderDetail in order.OrderDetails)
                {
                    var orderDetailDto = _mapper.Map<OrderDetailDto>(orderDetail);
                    ProductResponse product = await _grpcProductService.GetProduct(orderDetail.ProductId);
                    orderDetailDto.Thumbnail = product.Thumbnail;
                    orderDetailDto.Name = product.Name;
                    orderDetailDto.Price = product.Price;
                    orderDetailDto.Unit = product.Unit;
                    orderDetailDtos.Add(orderDetailDto);
                }
                orderDto.OrderDetails = orderDetailDtos;
                if (order.Voucher != null)
                    orderDto.VoucherDiscountPercent = order.Voucher.DiscountPercent;
                else
                    orderDto.VoucherDiscountPercent = 0;
                orderDtos.Add(orderDto);
            }
            return orderDtos;
        }

        public async Task<OrderDto> FindById(int id)
        {
            var order = await _orderRepository.FindById(id);
            var orderDto = _mapper.Map<OrderDto>(order);
            if (order.VoucherId != null)
            {
                orderDto.VoucherDiscountPercent = order.Voucher.DiscountPercent;
                orderDto.VoucherName = order.Voucher.Name;
                orderDto.VoucherMaxDiscount = order.Voucher.MaxDiscount;
            }
            UserResponse user = await _grpcUserService.GetUser(orderDto.UserId);
            orderDto.UserName = user.UserName;
            var orderDetailDtos = new List<OrderDetailDto>();
            foreach (var orderDetail in order.OrderDetails)
            {
                var orderDetailDto = _mapper.Map<OrderDetailDto>(orderDetail);
                ProductResponse product = await _grpcProductService.GetProduct(orderDetail.ProductId);
                orderDetailDto.Thumbnail = product.Thumbnail;
                orderDetailDto.Name = product.Name;
                orderDetailDto.Price = product.Price;
                orderDetailDto.Unit = product.Unit;
                orderDetailDtos.Add(orderDetailDto);
            }
            orderDto.OrderDetails = orderDetailDtos;
            return orderDto;
        }

        public async Task<bool> Save(OrderDto dto)
        {
            var result = false;
            var order = _mapper.Map<Order>(dto);
            order.CreatedTime = DateTime.Now;
            order.UpdatedTime = DateTime.Now;
            order.Status = OrderStatus.PENDING;
            if (order.VoucherId == 0) order.VoucherId = null;
            var orderDetails = new List<OrderDetail>();
            foreach (var detail in dto.OrderDetails)
            {
                orderDetails.Add(_mapper.Map<OrderDetail>(detail));
            }
            result = await _orderRepository.CreateOne(order) > 0;
            if (result)
            {
                foreach (var orderDetail in order.OrderDetails)
                {
                    var publishDto = new CartItemPublishDto() { UserId = order.UserId, ProductId = orderDetail.ProductId };
                    _messageProducer.SendMessage<CartItemPublishDto>(EventType.RemoveCartItem, publishDto);
                }
                if (order.VoucherId != null)
                {
                    var voucher = await _voucherService.FindById((int)order.VoucherId);
                    voucher.UsedQuantity += 1;
                    await _voucherService.Save(voucher);
                }

            }
            return result;
        }

        public async Task<bool> Update(int id, int status)
        {
            var order = await _orderRepository.FindById(id);
            if (order == null) return false;
            if(status == 2)
                order.Status = OrderStatus.WAITING;
            if (status == 3)
                order.Status = OrderStatus.DELIVERING;
            if (status == 4)
                order.Status = OrderStatus.RECEIVED;
            if (status == 5)
                order.Status = OrderStatus.CANCELED;
            await _orderRepository.SaveChange();
            return true;
        }
    }
}
