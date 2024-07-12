using CartService.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CartService.Services
{
    public interface ICartItemService
    {
        Task<bool> Add(CartItemDto dto);
        Task<bool> Update(CartItemDto dto);
        Task<bool> Delete(string userId, int productId);
        Task<List<CartItemDto>> FindByUserId(string userId);
    }
}
