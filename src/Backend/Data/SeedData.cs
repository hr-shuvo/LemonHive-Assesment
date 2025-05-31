using Backend.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Entities;

namespace Backend.Data;

public class SeedData
{
    public static async Task SeedProductsAsync(ApplicationDbContext context)
    {
        try
        {
            await context.Database.MigrateAsync();

            if (!context.Products.Any())
            {
                var products = new[]
                {
                new Product
                {
                    Name = "DJI Phantom 2", Price = 500, DiscountStartDate = DateTime.UtcNow.AddDays(-1),
                    DiscountEndDate = DateTime.UtcNow.AddDays(10), DiscountPercentage = 4
                },
                new Product
                {
                    Name = "DJI Phantom 4", Price = 1449, DiscountStartDate = DateTime.UtcNow,
                    DiscountEndDate = DateTime.UtcNow.AddDays(20), DiscountPercentage = 5
                },
                new Product
                {
                    Name = "DJI Phantom 1 Vision", Price = 739, DiscountStartDate = DateTime.UtcNow,
                    DiscountEndDate = DateTime.UtcNow.AddDays(2), DiscountPercentage = 5
                },
                new Product
                {
                    Name = "DJI Phantom 4 Pro", Price = 399, DiscountStartDate = DateTime.UtcNow,
                    DiscountEndDate = DateTime.UtcNow.AddDays(20), DiscountPercentage = 12
                },
                new Product
                {
                    Name = "4 Series - Intelligent", Price = 98, DiscountStartDate = DateTime.UtcNow,
                    DiscountEndDate = DateTime.UtcNow.AddDays(20), DiscountPercentage = 10
                },
                new Product
                {
                    Name = "4 Series - Amaging", Price = 739, DiscountStartDate = DateTime.UtcNow,
                    DiscountEndDate = DateTime.UtcNow.AddDays(2), DiscountPercentage = 20
                }
            };

                context.Products.AddRange(products);
                await context.SaveChangesAsync();
            }

        }
        catch (Exception)
        {
            throw new Exception("Failed to migrate/seed db, check db connection or check log");
        }

    }

    public static async Task SeedCartsAsync(WebApplication app)
    {
        try
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
                        new CartItem { ProductId = 1, Quantity = 7 , Name = "DJI Phantom 2", Price=500},
                        new CartItem { ProductId = 2, Quantity = 3 , Name="DJI Phantom 4", Price=1499},
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
        catch (Exception)
        {
            throw new Exception("Failed to migrate/seed mongo db, check db connection or check log");
        }

    }
}