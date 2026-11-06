namespace Bookstore.Api.Extensions;

public static class ProblemDetailsExtension
{
    public static IServiceCollection AddAppProblemDetails(this IServiceCollection service)
    {
        service.AddProblemDetails(options =>
            options.CustomizeProblemDetails = ctx =>
            {
                var pd = ctx.ProblemDetails;
                if (!pd.Extensions.ContainsKey("code"))
                {
                    pd.Extensions["code"] = pd.Status switch
                    {
                        StatusCodes.Status400BadRequest => "BAD_REQUEST",
                        StatusCodes.Status404NotFound => "NOT_FOUND",
                        StatusCodes.Status409Conflict => "CONFLICT",
                        StatusCodes.Status401Unauthorized => "UNAUTHORIZED",
                        StatusCodes.Status403Forbidden => "FORBIDDEN",
                        StatusCodes.Status500InternalServerError => "INTERNAL_SERVER_ERROR",
                        _ => "ERROR",
                    };
                }
            }
        );

        return service;
    }
}
