using AutoMapper;
using CartService.Dtos;
using CartService.Models;
using CartService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartService.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public CartItemService(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<bool> Add(CartItemDto dto)
        {
            var cartItem = await _cartRepository.GetCartItemByUserIdAndProductId(dto.UserId, dto.ProductId);
            if (cartItem != null)
            {
                dto.Quantity += cartItem.Quantity;
                return await _cartRepository.UpdateCartItem(_mapper.Map<CartItem>(dto)) > 0;
            }
            return await _cartRepository.AddCartItem(_mapper.Map<CartItem>(dto)) > 0;
        }

        public async Task<bool> Delete(string userId, int productId)
        {
            return await _cartRepository.DeleteCartItem(userId, productId) > 0;
        }

        public async Task<List<CartItemDto>> FindByUserId(string userId)
        {
            var cartItems = await _cartRepository.GetAllCartItemsByUserId(userId);
            var cartItemDtos = new List<CartItemDto>();
            foreach (var item in cartItems)
            {
                var dto = _mapper.Map<CartItemDto>(item);
                cartItemDtos.Add(dto);
            }
            return cartItemDtos;
        }

        public async Task<bool> Update(CartItemDto dto)
        {
            return await _cartRepository.UpdateCartItem(_mapper.Map<CartItem>(dto)) > 0;
        }
    }
}
