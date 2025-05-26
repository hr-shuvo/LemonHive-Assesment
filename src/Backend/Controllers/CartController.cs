using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetCart(string userId)
    {
        var cart = await _cartService.GetCartByUserIdAsync(userId);
        return Ok(cart);
    }

    [HttpPost("{userId}/add")]
    public async Task<IActionResult> AddOrUpdateItem(string userId, [FromBody] CartItem item)
    {
        await _cartService.AddOrUpdateItemAsync(userId, item.ProductId, item.Quantity);
        
        return Ok(new { message = "Item updated successfully." });
    }

    [HttpDelete("{userId}/remove/{productId}")]
    public async Task<IActionResult> RemoveItem(string userId, long productId)
    {
        await _cartService.RemoveItemAsync(userId, productId);
        
        return Ok(new { message = "Item removed successfully." });
    }

    [HttpDelete("{userId}/clear")]
    public async Task<IActionResult> ClearCart(string userId)
    {
        await _cartService.ClearCartAsync(userId);
        
        return Ok(new { message = "Cart cleared successfully." });
    }
}