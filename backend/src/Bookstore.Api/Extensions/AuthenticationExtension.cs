using Bookstore.Api.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Bookstore.Api.Extensions;

public static class AuthenticationExtension
{
    public static IServiceCollection AddAppAuthentication(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        var clerk = config.GetSection("Clerk");
        var authority =
            clerk.GetValue<string>("Authority")
            ?? throw new InvalidOperationException("Clerk:Authority is not configured.");
        var authorizedParties =
            clerk.GetSection("AuthorizedParties").Get<string[]>()
            ?? throw new InvalidOperationException("Clerk:AuthorizedParties is not configured.");

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = authority;
                options.MapInboundClaims = false;
                options.TokenValidationParameters = new()
                {
                    ValidateAudience = false,
                    RoleClaimType = AppClaims.AccountRole,
                };
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        return Task.CompletedTask;

                        //TODO: Uncomment this section if you want to restrict access based on the "azp" claim.

                        // var azp = context.Principal?.FindFirst("azp")?.Value;
                        // if (azp == null || !authorizedParties.Contains(azp))
                        //     context.Fail("Unauthorized azp claim.");
                        //
                        // return Task.CompletedTask;
                    },
                };
            });

        return services;
    }
}
