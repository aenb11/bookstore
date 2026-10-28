using Bookstore.Data.Common;

namespace Bookstore.Data.Extensions;

public static class SoftDeleteExtension
{
    public static IQueryable<T> Active<T>(this IQueryable<T> query)
        where T : ISoftDeletableEntity => query.Where(e => !e.IsDeleted);
}
