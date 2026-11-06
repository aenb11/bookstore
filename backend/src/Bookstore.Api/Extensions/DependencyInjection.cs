using Bookstore.Api.Services;
using Bookstore.Api.Services.Storage;

namespace Bookstore.Api.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<ICurrentAccountService, CurrentAccountService>();
        services.AddSingleton<IStorageService, LocalStorageService>();
        services.AddScoped<IReviewService, ReviewService>();

        return services;
    }
}
