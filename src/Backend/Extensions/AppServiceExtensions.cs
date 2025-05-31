using Backend.Data;
using Backend.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Backend.Extensions;

public static class AppServiceExtensions
{
    public static void AddApplicationCoreServices(this IServiceCollection services, IConfiguration config)
    {

        var connectionString = config.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(opt =>
        {
            opt.UseNpgsql(connectionString);
        });

        services.AddCors(opt =>
        {
            opt.AddPolicy("CorsPolicy", policy =>
            {
                policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000");
            });
        });






        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));



    }
}