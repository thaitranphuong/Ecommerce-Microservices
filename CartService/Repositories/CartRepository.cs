using CartService.Models;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace CartService.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly IDbConnection _dbConnection;

        public CartRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<CartItem>> GetAllCartItems()
        {
            var sql = "SELECT * FROM CartItems";
            return await _dbConnection.QueryAsync<CartItem>(sql);
        }

        public async Task<IEnumerable<CartItem>> GetAllCartItemsByUserId(string userId)
        {
            var sql = "SELECT * FROM CartItems WHERE UserId = @UserId";
            return await _dbConnection.QueryAsync<CartItem>(sql, new { UserId = userId });
        }

        public async Task<CartItem> GetCartItemByUserIdAndProductId(string userId, int productId)
        {
            var sql = "SELECT * FROM CartItems WHERE UserId = @UserId AND ProductId = @ProductId";
            return await _dbConnection.QueryFirstOrDefaultAsync<CartItem>(sql, new { UserId = userId, ProductId = productId });
        }

        public async Task<int> AddCartItem(CartItem cartItem)
        {
            var sql = "INSERT INTO CartItems (UserId, ProductId, Quantity) VALUES (@UserId, @ProductId, @Quantity)";
            return await _dbConnection.ExecuteAsync(sql, new { cartItem.UserId, cartItem.ProductId, cartItem.Quantity });
        }

        public async Task<int> UpdateCartItem(CartItem cartItem)
        {
            var sql = "UPDATE CartItems SET Quantity = @Quantity WHERE UserId = @UserId AND ProductId = @ProductId";
            return await _dbConnection.ExecuteAsync(sql, new { cartItem.UserId, cartItem.ProductId, cartItem.Quantity });
        }

        public async Task<int> DeleteCartItem(string userId, int productId)
        {
            var sql = "DELETE FROM CartItems WHERE UserId = @UserId AND ProductId = @ProductId";
            return await _dbConnection.ExecuteAsync(sql, new { UserId = userId, ProductId = productId });
        }
    }
}
