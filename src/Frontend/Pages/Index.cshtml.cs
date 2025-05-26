using Frontend.Common;
using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Frontend.Pages;

public class IndexModel : PageModel
{
    private readonly ProductService _productService;
    
    public PaginatedRequest Pagination { get; set; } = new();
    
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger, ProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    public PaginatedResult<Product>? Products { get; set; }

    public async Task OnGetAsync()
    {
        Products = await _productService.GetProductsAsync(Pagination.Query, Pagination.Page, Pagination.PageSize);
        
        if (Products == null)
        {
            _logger.LogWarning("No products found for the given query: {Query}", Pagination.Query);
            
            Products = new PaginatedResult<Product>
            {
                CurrentPage = Pagination.Page,
                PageSize = Pagination.PageSize,
                TotalItems = 0,
                TotalPages = 0,
                Data = []
            };
        }
        else
        {
            _logger.LogInformation("Fetched {Count} products for page {Page} with page size {PageSize}", 
                Products.Data.Count, Pagination.Page, Pagination.PageSize);
        }
    }
}