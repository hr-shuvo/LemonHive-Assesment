using Frontend.Services;

namespace Frontend.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddHttpClient("BackendApi", client =>
        {
            client.BaseAddress = new Uri("http://localhost:5001/api/");
        });
        
        services.AddScoped<ProductService>();
        services.AddScoped<CartService>();
    }
}