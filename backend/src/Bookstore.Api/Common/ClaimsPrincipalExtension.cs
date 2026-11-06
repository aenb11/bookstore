using System.Security.Claims;

namespace Bookstore.Api.Common;

public static class ClaimsPrincipalExtensions
{
    public static int GetAccountId(this ClaimsPrincipal user)
    {
        var value = user.FindFirstValue(AppClaims.AccountId);
        return int.TryParse(value, out var id)
            ? id
            : throw new InvalidOperationException("Missing account_id claim.");
    }
}
