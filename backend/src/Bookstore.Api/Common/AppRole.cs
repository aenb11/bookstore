using Bookstore.Data.Entities;

namespace Bookstore.Api.Common;

public static class AppRole
{
    public const string Admin = nameof(AccountRole.Admin);
    public const string Staff = nameof(AccountRole.Staff);
    public const string Customer = nameof(AccountRole.Customer);
}
