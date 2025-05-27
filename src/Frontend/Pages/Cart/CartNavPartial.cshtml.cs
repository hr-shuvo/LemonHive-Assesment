using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Frontend.Pages.Cart;

public class CartNavPartialModel : PageModel
{
    private readonly CartService _cartService;
    
    public CartModel Cart { get; set; } = new();

    public CartNavPartialModel(CartService cartService)
    {
        _cartService = cartService;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var cart = await _cartService.GetCartAsync();
        
        return new PartialViewResult
        {
            ViewName = "_CartNavPartial",
            ViewData = new ViewDataDictionary<CartModel>(ViewData, cart)
        };
    }
}