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
    public int PageSize { get; set; } = 10;

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
        ViewData["ApiUrl"] = "http://localhost:5001/api";
        
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




    #region Cart

    public IActionResult OnPostAddToCart(int productId)
    {
        // if (!Cart.ContainsKey(productId))
        //     Cart[productId] = 1;

        return RedirectToPage();
    }
    
    public IActionResult OnPostIncreaseQuantity(int productId)
    {
        // if (Cart.ContainsKey(productId))
        //     Cart[productId]++;
        // else
        //     Cart[productId] = 1;

        return RedirectToPage();
    }
    
    public IActionResult OnPostDecreaseQuantity(int productId)
    {
        // if (Cart.ContainsKey(productId))
        // {
        //     Cart[productId]--;
        //
        //     if (Cart[productId] <= 0)
        //         Cart.Remove(productId);
        // }

        return RedirectToPage();
    }
    
    private List<Product> LoadProducts()
    {
        // Example static data
        return new List<Product>
        {
            new Product { Id = 1, Name = "DJI Phantom 2 Vision+", OriginalPrice = 499, DiscountPercentage = 17, IsDiscounted = true, ImageUrl = "phantom2.jpg" },
            new Product { Id = 2, Name = "DJI Phantom 4 Multispectral", OriginalPrice = 1449, IsDiscounted = false, ImageUrl = "phantom4.jpg" }
        };
    }

    #endregion
}