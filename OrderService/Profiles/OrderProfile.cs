using AutoMapper;
using OrderService.Dtos;
using OrderService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderDto, Order>();
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => new List<OrderDetailDto>()));
        }
    }
}
