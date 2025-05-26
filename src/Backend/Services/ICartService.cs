using Backend.Models;

namespace Backend.Services;

public interface ICartService
{
    Task<Cart> GetCartByUserIdAsync(string userId);
    Task AddOrUpdateItemAsync(string userId, long productId, int quantity);
    Task RemoveItemAsync(string userId, long productId);
    Task ClearCartAsync(string userId);
}