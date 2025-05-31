using System.Linq.Expressions;
using Backend.Data;
using Backend.Dtos;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;
    private readonly IBaseRepository<Product> _repo;

    public ProductService(ApplicationDbContext context, IBaseRepository<Product> repository)
    {
        _context = context;
        _repo = repository;
    }

    public async Task<(List<ProductDto> items, int page, int size, int totalPage, int totalCount)> LoadProductsAsync(int page, int pageSize)
    {
        // var totalItems = await _context.Products.CountAsync();
        // var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        // var products = await _context.Products
        //     .Skip((page - 1) * pageSize)
        //     .Take(pageSize)
        //     .ToListAsync();

        Expression<Func<Product, bool>> filter = x => true;

        var result = await _repo.LoadAsync(filter, page, pageSize);

        var afterDiscount = result.DataList.Select(ApplyDiscount).ToList();
        
        return (afterDiscount, page, pageSize, result.TotalPage, result.TotalCount);
    }

    public async Task<ProductDto> GetProductByIdAsync(long id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return null;

        var afterDiscount = ApplyDiscount(product);
        
        return afterDiscount;
    }

    public async Task UpsertProductAsync(ProductUpsertDto dto)
    {
        Product product;

        if (dto.Id is > 0)
        {
            product = await _context.Products.FindAsync(dto.Id.Value);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {dto.Id} not found.");

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
    }

    public async Task DeleteProductAsync(long id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            throw new KeyNotFoundException($"Product with ID {id} not found.");

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }

    public async Task<(List<ProductDto> items, int page, int size, int totalPage, int totalCount)> SearchProductsAsync(string searchTerm, int page, int pageSize)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            searchTerm = "";

        searchTerm = searchTerm.ToLower();

        Expression<Func<Product, bool>> filter = x => x.Name.ToLower().Contains(searchTerm) ||
        x.Description.ToLower().Contains(searchTerm);

        var result = await _repo.LoadAsync(filter, page, pageSize);

        var afterDiscount = result.DataList.Select(ApplyDiscount).ToList();
        
        return (afterDiscount, page, pageSize, result.TotalPage, result.TotalCount);
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
            FinalPrice = finalPrice,
            IsDiscounted = isDiscounted
        };
    }

    

    #endregion
}