using Backend.Models;
using MongoDB.Driver;
using MongoDB.Entities;

namespace Backend.Data;

public class SeedData
{
    public static async Task SeedProductsAsync(ApplicationDbContext context)
    {
        if (!context.Products.Any())
        {
            var products = new[]
            {
                new Product
                {
                    Name = "Sample Product 1", Price = 100, DiscountStartDate = DateTime.UtcNow.AddDays(-1),
                    DiscountEndDate = DateTime.UtcNow.AddDays(1), DiscountPercentage = 10
                },
                new Product
                {
                    Name = "Sample Product 2", Price = 200, DiscountStartDate = DateTime.UtcNow,
                    DiscountEndDate = DateTime.UtcNow.AddDays(2), DiscountPercentage = 20
                }
            };

            context.Products.AddRange(products);
            await context.SaveChangesAsync();
        }
    }

    public static async Task SeedCartsAsync(WebApplication app)
    {
        var connectionString = app.Configuration.GetConnectionString("MongoDbConnection");
        await DB.InitAsync("CartDb", MongoClientSettings.FromConnectionString(connectionString));
        
        var existingCarts = await DB.Find<Cart>().Limit(1).ExecuteAsync();
        if (existingCarts.Count == 0)
        {
            var cart1 = new Cart
            {
                // UserId = "",
                Items =
                [
                    new CartItem { ProductId = 1, Quantity = 7 },
                    new CartItem { ProductId = 2, Quantity = 3 }
                ]
            };

            var cart2 = new Cart
            {
                Items = [new CartItem() { ProductId = 2, Quantity = 5 }]
            };

            await cart1.SaveAsync();
            await cart2.SaveAsync();
        }
    }
}