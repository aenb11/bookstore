using Microsoft.AspNetCore.Diagnostics;

namespace Bookstore.Api.Common;

public class AppExceptionHandler(IProblemDetailsService problems) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext ctx, Exception ex, CancellationToken ct)
    {
        if (ex is not AppException app)
            return false;

        ctx.Response.StatusCode = app.Status;
        return await problems.TryWriteAsync(
            new ProblemDetailsContext
            {
                HttpContext = ctx,
                ProblemDetails =
                {
                    Status = app.Status,
                    Title = app.Message,
                    Extensions = { ["code"] = app.Code },
                },
            }
        );
    }
}
