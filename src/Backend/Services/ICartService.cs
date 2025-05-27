using Backend.Models;

namespace Backend.Services;

public interface ICartService
{
    Task<Cart> GetCartByUserIdAsync(string userId);
    Task IncreaseItemAsync(string userId, long productId);
    Task DecreaseItemAsync(string userId, long productId);
    Task RemoveItemAsync(string userId, long productId);
    Task ClearCartAsync(string userId);
}