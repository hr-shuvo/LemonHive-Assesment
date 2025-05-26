using Backend.Data;
using Backend.Dtos;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProductsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(int page = 1, int pageSize = 10)
    {
        var totalItems = await _context.Products.CountAsync();
        var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        var products = await _context.Products
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var afterDiscount = products.Select(ApplyDiscount);

        return Ok(new
        {
            currentPage = page,
            pageSize,
            totalItems,
            totalPages,
            data = afterDiscount
        });
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        var afterDiscount = ApplyDiscount(product);
        return Ok(afterDiscount);
    }


    [HttpPost("upsert")]
    public async Task<IActionResult> UpsertProduct([FromBody] ProductUpsertDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        Product product;

        if (dto.Id is > 0)
        {
            product = await _context.Products.FindAsync(dto.Id.Value);
            if (product == null)
                return NotFound("Product not found");

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.DiscountPercentage = dto.DiscountPercentage;
            product.DiscountStartDate = dto.DiscountStartDate;
            product.DiscountEndDate = dto.DiscountEndDate;

            _context.Products.Update(product);
        }
        else
        {
            product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                DiscountPercentage = dto.DiscountPercentage,
                DiscountStartDate = dto.DiscountStartDate,
                DiscountEndDate = dto.DiscountEndDate
            };

            _context.Products.Add(product);
        }

        await _context.SaveChangesAsync();
        return Ok(product);
    }


    [HttpGet("search")]
    public async Task<IActionResult> Search(string query, int page = 1, int pageSize = 10)
    {
        if (string.IsNullOrWhiteSpace(query))
            query = "";

        query = query.ToLower();

        var filtered = _context.Products
            .Where(p => p.Name.ToLower().Contains(query) || p.Description.ToLower().Contains(query));

        var totalItems = await filtered.CountAsync();
        var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        var results = await filtered
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var afterDiscount = results.Select(ApplyDiscount);

        return Ok(new
        {
            currentPage = page,
            pageSize,
            totalItems,
            totalPages,
            data = afterDiscount
        });
    }


    #region Private Methods
    
    private static ProductDto ApplyDiscount(Product product)
    {
        var now = DateTime.UtcNow;

        var isDiscounted = product.DiscountStartDate.HasValue &&
                           product.DiscountEndDate.HasValue &&
                           product.DiscountPercentage.HasValue &&
                           product.DiscountStartDate <= now &&
                           now <= product.DiscountEndDate;

        var finalPrice = isDiscounted
            ? product.Price - (product.Price * product.DiscountPercentage.Value / 100)
            : product.Price;

        return new ProductDto()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            OriginalPrice = product.Price,
            DiscountPercentage = product.DiscountPercentage,
            DiscountStartDate = product.DiscountStartDate,
            DiscountEndDate = product.DiscountEndDate,
            FinalPrice = finalPrice
        };
    }

    

    #endregion

    
}