namespace Bookstore.Api.Extensions;

public static class CorsExtension
{
    public const string FrontendPolicy = "FrontendPolicy";

    public static IServiceCollection AddAppCors(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        var allowedOrigins = config.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

        services.AddCors(options =>
        {
            options.AddPolicy(
                name: FrontendPolicy,
                policy =>
                {
                    if (allowedOrigins.Length > 0)
                    {
                        policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod();
                    }
                    else
                    {
                        policy.SetIsOriginAllowed(_ => false);
                    }
                }
            );
        });

        return services;
    }
}
