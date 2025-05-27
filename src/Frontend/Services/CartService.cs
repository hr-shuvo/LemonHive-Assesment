using Frontend.Models;

namespace Frontend.Services;

public class CartService
{
    private readonly HttpClient _httpClient;

    public CartService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("BackendApi");
    }
    
    public async Task<CartModel> GetCartAsync()
    {
        var url = "cart";
        
        try
        {
            var response = await _httpClient.GetAsync(url);
            
            if (response.IsSuccessStatusCode)
            {
                var cart = await response.Content.ReadFromJsonAsync<CartModel>();
                
                Console.WriteLine("Cart: " + cart);
                return cart ?? new CartModel();
            }

            return new CartModel();
        }
        catch
        {
            return new CartModel();
        }
    }
    
    public async Task<bool> AddToCartAsync(int productId)
    {
        var url = "api/cart/increase/" + productId;
        var response = await _httpClient.PostAsJsonAsync(url,new {});
        
        return response.IsSuccessStatusCode;
    }
}