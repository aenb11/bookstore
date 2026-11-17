using Bookstore.Data.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bookstore.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddDataInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        bool isDevelopment
    )
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(
                connectionString,
                npgsql =>
                {
                    npgsql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                    npgsql.EnableRetryOnFailure(3);
                }
            );
            options.UseSnakeCaseNamingConvention();

            if (isDevelopment)
            {
                options.EnableSensitiveDataLogging();
                options.UseAsyncSeeding(DevDataSeeder.SeedAsync);
                options.UseSeeding(
                    (ctx, _) =>
                        DevDataSeeder
                            .SeedAsync(ctx, _, CancellationToken.None)
                            .GetAwaiter()
                            .GetResult()
                );
            }
        });

        return services;
    }
}
