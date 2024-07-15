using AutoMapper;
using CartService.Dtos;
using CartService.Models;
using CartService.Repositories;
using CartService.SyncServices;
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
        private readonly ICacheService _cacheService;
        private readonly IGrpcProductService _grpcProductService;

        public CartItemService(ICartRepository cartRepository, IMapper mapper, ICacheService cacheService, IGrpcProductService grpcProductService)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
            _cacheService = cacheService;
            _grpcProductService = grpcProductService;
        }

        public async Task<bool> Add(CartItemDto dto)
        {
            var cacheKey = $"cart_{dto.UserId}";
            await _cacheService.RemoveDataAsync(cacheKey);
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
            var cacheKey = $"cart_{userId}";
            await _cacheService.RemoveDataAsync(cacheKey);
            return await _cartRepository.DeleteCartItem(userId, productId) > 0;
        }

        public async Task<List<CartItemDto>> FindByUserId(string userId)
        {
            var cacheKey = $"cart_{userId}";
            var cartItemDtos = await _cacheService.GetDataAsync<List<CartItemDto>>(cacheKey);

            if (cartItemDtos != null)
            {
                Console.WriteLine("Caching");
                return cartItemDtos;
            }

            var cartItems = await _cartRepository.GetAllCartItemsByUserId(userId);
            cartItemDtos = new List<CartItemDto>();
            foreach (var item in cartItems)
            {
                ProductResponse product = await _grpcProductService.GetProduct(item.ProductId);
                var dto = _mapper.Map<CartItemDto>(item);
                dto.Name = product.Name;
                dto.Price = product.Price;
                dto.Thumbnail = product.Thumbnail;
                cartItemDtos.Add(dto);
            }

            await _cacheService.SetDataAsync(cacheKey, cartItemDtos, TimeSpan.FromMinutes(5));
            return cartItemDtos;
        }

        public async Task<bool> Update(CartItemDto dto)
        {
            var cacheKey = $"cart_{dto.UserId}";
            await _cacheService.RemoveDataAsync(cacheKey);
            return await _cartRepository.UpdateCartItem(_mapper.Map<CartItem>(dto)) > 0;
        }
    }
}
