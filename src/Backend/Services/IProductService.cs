using Backend.Dtos;

namespace Backend.Services;

public interface IProductService
{
    Task<(List<ProductDto> items, int page, int size, int totalPage, int totalCount)> LoadProductsAsync(int page, int pageSize);
    Task<ProductDto> GetProductByIdAsync(long id);
    Task UpsertProductAsync(ProductUpsertDto dto);
    Task DeleteProductAsync(long id);
    Task<(List<ProductDto> items, int page, int size, int totalPage, int totalCount)> SearchProductsAsync(string searchTerm, int page, int pageSize);
}