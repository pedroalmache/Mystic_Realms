using Microsoft.EntityFrameworkCore;
using MysticRealms.Infrastructure.Persistence;

namespace MysticRealms.ServerApi.Extensions;

public static class DbContextExtensions
{
    public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}