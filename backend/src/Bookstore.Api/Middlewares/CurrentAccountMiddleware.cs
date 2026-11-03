using System.Security.Claims;
using Bookstore.Api.Common;
using Bookstore.Api.Services;
using Bookstore.Data.Entities;

namespace Bookstore.Api.Middlewares;

public class CurrentAccountMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, ICurrentAccountService currentAccountService)
    {
        if (context.User.Identity?.IsAuthenticated != true)
        {
            await next(context);
            return;
        }

        var acc = await currentAccountService.GetOrCreateCurrentAccountAsync();
        if (acc.IsDeleted || acc.Status != AccountStatus.Active)
        {
            var problems = context.RequestServices.GetRequiredService<IProblemDetailsService>();
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await problems.WriteAsync(
                new ProblemDetailsContext
                {
                    HttpContext = context,
                    ProblemDetails =
                    {
                        Status = StatusCodes.Status403Forbidden,
                        Title = "Account is not active",
                        Extensions = { ["code"] = "ACCOUNT_INACTIVE" },
                    },
                }
            );
            return;
        }

        var identity = (ClaimsIdentity)context.User.Identity;
        identity.AddClaim(new Claim(AppClaims.AccountId, acc.Id.ToString()));
        identity.AddClaim(new Claim(AppClaims.AccountRole, acc.Role.ToString()));
        await next(context);
    }
}
