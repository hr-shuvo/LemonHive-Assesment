using Frontend.Common;
using Frontend.Models;

namespace Frontend.Services;

public class ProductService
{
    private readonly HttpClient _httpClient;
    
    public ProductService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("BackendApi");
    }
    
    
    public async Task<PaginatedResult<Product>?> GetProductsAsync(string? query, int page = 1, int pageSize = 10)
    {
        var url = $"products?query={Uri.EscapeDataString(query ?? string.Empty)}&page={page}&pageSize={pageSize}";
        var response = await _httpClient.GetAsync(url);
        
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error fetching products: {response.ReasonPhrase}");
        }
        
        var result = await response.Content.ReadFromJsonAsync<PaginatedResult<Product>>();
        
        return result;
    }
}