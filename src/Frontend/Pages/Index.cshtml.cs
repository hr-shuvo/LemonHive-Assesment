using Frontend.Common;
using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Frontend.Pages;

public class IndexModel : PageModel
{
    private readonly ProductService _productService;
    
    // public PaginatedRequest Pagination { get; set; } = new();
    
    [BindProperty(SupportsGet = true)]
    public int PageNumber { get; set; } = 1;

    [BindProperty(SupportsGet = true)]
    public int PageSize { get; set; } = 2;

    [BindProperty(SupportsGet = true)]
    public string? Query { get; set; }

    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger, ProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    public PaginatedResult<Product>? Products { get; set; }

    public async Task OnGetAsync()
    {
        if (PageNumber < 1) PageNumber = 1;
        if (PageSize <= 0) PageSize = 2;
        
        _logger.LogInformation("Fetching products for query: {Query}, page: {PageNumber}, page size: {PageSize}", 
            Query, PageNumber, PageSize);
        
        Products = await _productService.GetProductsAsync(Query, PageNumber, PageSize);
        
        if (Products == null)
        {
            _logger.LogWarning("No products found for the given query: {Query}", Query);
            
            Products = new PaginatedResult<Product>
            {
                CurrentPage = PageNumber,
                PageSize = PageSize,
                TotalItems = 0,
                TotalPages = 0,
                Data = []
            };
        }
        else
        {
            _logger.LogInformation("Fetched {Count} products for page {PageNumber} with page size {PageSize}", 
                Products.Data.Count, PageNumber, PageSize);
        }
    }
}