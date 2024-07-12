using AutoMapper;
using CartService.Dtos;
using CartService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartService.Profiles
{
    public class CartItemProfile : Profile
    {
        public CartItemProfile()
        {
            CreateMap<CartItemDto, CartItem>();

            CreateMap<CartItem, CartItemDto>();
        }
    }
}
