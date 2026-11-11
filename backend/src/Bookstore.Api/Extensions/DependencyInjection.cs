using Bookstore.Api.Services;
using Bookstore.Api.Services.Management;
using Bookstore.Api.Services.Storage;
using Bookstore.Api.Services.Storefront;

namespace Bookstore.Api.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<ICurrentAccountService, CurrentAccountService>();
        services.AddSingleton<IStorageService, LocalStorageService>();

        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<ITagService, TagService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ISerieService, SerieService>();
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<IPublisherService, PublisherService>();
        services.AddScoped<IAddressService, AddressService>();

        services.AddScoped<IReviewManagementService, ReviewManagementService>();
        services.AddScoped<ITagManagementService, TagManagementService>();
        services.AddScoped<ICategoryManagementService, CategoryManagementService>();
        services.AddScoped<ISerieManagementService, SerieManagementService>();
        services.AddScoped<IAuthorManagementService, AuthorManagementService>();
        services.AddScoped<IPublisherManagementService, PublisherManagementService>();
        services.AddScoped<IAccountManagementService, AccountManagementService>();

        return services;
    }
}
