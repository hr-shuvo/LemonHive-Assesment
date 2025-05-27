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

    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        var userId = "1000";
        
        var cart = await _cartService.GetCartByUserIdAsync(userId);
        return Ok(cart);
    }

    [HttpPost("increase/{productId}")]
    public async Task<IActionResult> IncreaseItem(long productId)
    {
        var userId = "1000";
        await _cartService.IncreaseItemAsync(userId, productId);
        
        return Ok(new { message = "Item added successfully." });
    }

    [HttpDelete("decrease/{productId}")]
    public async Task<IActionResult> DecreaseItem(long productId)
    {
        var userId = "1000";
        await _cartService.DecreaseItemAsync(userId, productId);
        
        return Ok(new { message = "Item descreased successfully." });
    }
    
    [HttpDelete("remove/{productId}")]
    public async Task<IActionResult> RemoveItem(long productId)
    {
        var userId = "1000";
        await _cartService.RemoveItemAsync(userId, productId);
        
        return Ok(new { message = "Item removed successfully." });
    }

    [HttpDelete("clear")]
    public async Task<IActionResult> ClearCart()
    {
        var userId = "1000";
        
        await _cartService.ClearCartAsync(userId);
        
        return Ok(new { message = "Cart cleared successfully." });
    }
}