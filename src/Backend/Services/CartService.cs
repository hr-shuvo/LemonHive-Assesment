using Backend.Models;
using MongoDB.Entities;

namespace Backend.Services;

public class CartService : ICartService
{
    public async Task<Cart> GetCartByUserIdAsync(string userId)
    {
        var cart = await DB.Find<Cart>().Match(c => c.UserId == userId).ExecuteFirstAsync();
        
        if (cart == null)
        {
            cart = new Cart { UserId = userId };
            await cart.SaveAsync();
        }
        
        return cart;
    }

    public async Task AddOrUpdateItemAsync(string userId, long productId, int quantity)
    {
        var cart = await GetCartByUserIdAsync(userId);
        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

        if (item == null)
            cart.Items.Add(new CartItem { ProductId = productId, Quantity = quantity });
        else
            item.Quantity = quantity;

        await cart.SaveAsync();
    }

    public async Task RemoveItemAsync(string userId, long productId)
    {
        var cart = await GetCartByUserIdAsync(userId);
        cart.Items.RemoveAll(i => i.ProductId == productId);
        
        await cart.SaveAsync();
    }

    public async Task ClearCartAsync(string userId)
    {
        var cart = await GetCartByUserIdAsync(userId);
        cart.Items.Clear();
        await cart.SaveAsync();
    }
}