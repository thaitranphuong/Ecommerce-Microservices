using AutoMapper;
using OrderService.Dtos;
using OrderService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Profiles
{
    public class OrderDetailProfile : Profile
    {
        public OrderDetailProfile()
        {
            CreateMap<OrderDetailDto, OrderDetail>();
            CreateMap<OrderDetail, OrderDetailDto>();
        }
    }
}
