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
        private readonly IMailService _mailService;

        public OrderServices(IOrderRepository orderRepository, IMapper mapper, IGrpcProductService grpcProductService, IMessageProducer messageProducer, IVoucherService voucherService, IGrpcUserService grpcUserService, IMailService mailService)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _grpcProductService = grpcProductService;
            _messageProducer = messageProducer;
            _voucherService = voucherService;
            _grpcUserService = grpcUserService;
            _mailService = mailService;
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
                    //var orderDetailDto = _mapper.Map<OrderDetailDto>(orderDetail);
                    var orderDetailDto = new OrderDetailDto()
                    {
                        ProductId = orderDetail.ProductId,
                        OrderId = orderDetail.OrderId,
                        Price = orderDetail.Price,
                        Quantity = orderDetail.Quantity,
                        WarehouseId = orderDetail.WarehouseId,
                    };
                    ProductResponse product = await _grpcProductService.GetProduct(orderDetail.ProductId);
                    orderDetailDto.Thumbnail = product.Thumbnail;
                    orderDetailDto.Name = product.Name;
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
                    var orderDetailDto = new OrderDetailDto()
                    {
                        ProductId = orderDetail.ProductId,
                        OrderId = orderDetail.OrderId,
                        Price = orderDetail.Price,
                        Quantity = orderDetail.Quantity,
                        WarehouseId = orderDetail.WarehouseId,
                    };
                    ProductResponse product = await _grpcProductService.GetProduct(orderDetail.ProductId);
                    orderDetailDto.Thumbnail = product.Thumbnail;
                    orderDetailDto.Name = product.Name;
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
                    var orderDetailDto = new OrderDetailDto()
                    {
                        ProductId = orderDetail.ProductId,
                        OrderId = orderDetail.OrderId,
                        Price = orderDetail.Price,
                        Quantity = orderDetail.Quantity,
                        WarehouseId = orderDetail.WarehouseId,
                    };
                    ProductResponse product = await _grpcProductService.GetProduct(orderDetail.ProductId);
                    orderDetailDto.Thumbnail = product.Thumbnail;
                    orderDetailDto.Name = product.Name;
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
                var orderDetailDto = new OrderDetailDto()
                {
                    ProductId = orderDetail.ProductId,
                    OrderId = orderDetail.OrderId,
                    Price = orderDetail.Price,
                    Quantity = orderDetail.Quantity,
                    WarehouseId = orderDetail.WarehouseId,
                };
                ProductResponse product = await _grpcProductService.GetProduct(orderDetail.ProductId);
                orderDetailDto.Thumbnail = product.Thumbnail;
                orderDetailDto.Name = product.Name;
                orderDetailDto.Unit = product.Unit;
                orderDetailDtos.Add(orderDetailDto);
            }
            orderDto.OrderDetails = orderDetailDtos;
            return orderDto;
        }

        public async Task<bool> Save(OrderDto dto)
        {
            var result = false;
            var orderDetailDtos = dto.OrderDetails;
            dto.OrderDetails = null;
            var order = _mapper.Map<Order>(dto);
            order.CreatedTime = DateTime.Now;
            order.UpdatedTime = DateTime.Now;
            order.Status = OrderStatus.PENDING;
            if (order.VoucherId == 0) order.VoucherId = null;
            var orderDetails = new List<Models.OrderDetail>();
            foreach (var detailDto in orderDetailDtos)
            {
                var detail = new Models.OrderDetail()
                {
                    ProductId = detailDto.ProductId,
                    Quantity = detailDto.Quantity,
                    Price = detailDto.Price,
                    WarehouseId = 0
                };
                orderDetails.Add(detail);
                order.OrderDetails.Add(detail);
            }
            result = await _orderRepository.CreateOne(order) > 0;
            if (result)
            {
                foreach (var orderDetail in order.OrderDetails)
                {
                    var publishDto = new CartItemPublishDto() { UserId = order.UserId, ProductId = orderDetail.ProductId };
                    _messageProducer.SendMessage<CartItemPublishDto>(EventType.RemoveCartItem, publishDto);
                }
                var reduceProductPublishDto = new List<ReduceProductPublishDto>();
                foreach (var orderDetail in order.OrderDetails)
                {
                    var publishDto = new ReduceProductPublishDto() { Id = orderDetail.ProductId, Quantity = orderDetail.Quantity };
                    reduceProductPublishDto.Add(publishDto);
                }
                _messageProducer.SendMessage<List<ReduceProductPublishDto>>(EventType.ReduceProductQuantity, reduceProductPublishDto);
                if (order.VoucherId != null)
                {
                    var voucher = await _voucherService.FindById((int)order.VoucherId);
                    voucher.UsedQuantity += 1;
                    await _voucherService.Save(voucher);
                }

            }
            return result;
        }

        public async Task SendMailWhenOrderUpdate(Order order, int status)
        {
            UserResponse user = await _grpcUserService.GetUser(order.UserId);
            var email = user.Email;
            var subject = "";
            var body = "Thông tin đơn hàng<br/>";
            int stt = 1;
            body += $"<div>Ngày đặt: {order.CreatedTime.Day}/{order.CreatedTime.Month}/{order.CreatedTime.Year}</div><br/>";
            foreach (var orderDetail in order.OrderDetails)
            {
                ProductResponse product = await _grpcProductService.GetProduct(orderDetail.ProductId);
                body += $"<div>{stt} | <img src='{product.Thumbnail}' width='100'/> | {product.Name} | Đơn giá: {product.Price}/{product.Unit} | Số lượng: {orderDetail.Quantity}{product.Unit}</div>";
                stt++;
            }
            body += $"<strong>Tổng cộng: {order.Total}</strong><br/>Nếu có thắc mắc, vui lòng liên hệ chúng tôi qua email này";
            if (status == 2)
                subject = "[Fruitable Shop] Đơn hàng đã được duyệt";
            if (status == 3)
                subject = "[Fruitable Shop] Đơn hàng đang giao";
            if (status == 5)
                subject = "[Fruitable Shop] Đơn hàng đã bị hủy";

            _ = _mailService.Send(new MailDto()
            {
                To = email,
                Subject = subject,
                Body = body
            });
        }

        public async Task<bool> Update(int id, int status)
        {
            var order = await _orderRepository.FindById(id);
            if (order == null) return false;
            if (status == 2)
                order.Status = OrderStatus.WAITING;
            if (status == 3)
                order.Status = OrderStatus.DELIVERING;
            if (status == 4)
                order.Status = OrderStatus.RECEIVED;
            if (status == 5)
                order.Status = OrderStatus.CANCELED;
            await _orderRepository.SaveChange();
            if (status == 2 || status == 3 || status == 5)
            {
               _ = SendMailWhenOrderUpdate(order, status);
            }
            return true;
        }

        public async Task<bool> UpdateOrderDetails(OrderDetailDto[] orderDetailDtos)
        {
            foreach(var dto in orderDetailDtos)
            {
                var orderDetail = await _orderRepository.FindOrderDetail(dto.OrderId, dto.ProductId);
                if(orderDetail != null)
                {
                    orderDetail.WarehouseId = dto.WarehouseId;
                    await _orderRepository.SaveChange();
                }
            }
            return true;
        }

        public async Task<List<OrderDto>> FindAllByYearToStatistic(int year)
        {
            var orders = await _orderRepository.FindAllByYearToStatistic(year);
            var orderDtos = new List<OrderDto>();
            foreach(var order in orders)
            {
                var orderDto = _mapper.Map<OrderDto>(order);
                orderDtos.Add(orderDto);
            }

            return orderDtos;
        }

        public async Task<List<OrderDto>> FindAllByDateToStatistic(DateTime startDate, DateTime endDate)
        {
            var orders = await _orderRepository.FindAllByDateToStatistic(startDate, endDate);
            var orderDtos = new List<OrderDto>();
            foreach (var order in orders)
            {
                var orderDto = _mapper.Map<OrderDto>(order);
                orderDtos.Add(orderDto);
            }

            return orderDtos;
        }

        public async Task<List<OrderDto>> FindAllByMonthToStatistic(int month, int year)
        {
            var orders = await _orderRepository.FindAllByMonthToStatistic(month, year);
            var orderDtos = new List<OrderDto>();
            foreach (var order in orders)
            {
                var orderDto = _mapper.Map<OrderDto>(order);
                orderDtos.Add(orderDto);
            }

            return orderDtos;
        }
    }
}
