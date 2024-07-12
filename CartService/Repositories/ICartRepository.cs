using CartService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartService.Repositories
{
    public interface ICartRepository
    {
        Task<IEnumerable<CartItem>> GetAllCartItems();

        Task<IEnumerable<CartItem>> GetAllCartItemsByUserId(string userId);

        Task<CartItem> GetCartItemByUserIdAndProductId(string userId, int productId);

        Task<int> AddCartItem(CartItem cartItem);

        Task<int> UpdateCartItem(CartItem cartItem);

        Task<int> DeleteCartItem(string userId, int productId);
    }
}
