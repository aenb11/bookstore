using System.Linq.Expressions;
using Bookstore.Api.Common.Enums;
using Bookstore.Api.Common.Extensions;
using Bookstore.Api.DTOs.Tags;
using Bookstore.Data.Entities;

namespace Bookstore.Api.Queries;

public static class TagQueryExtenstion
{
    private static readonly Dictionary<TagSortBy, Expression<Func<Tag, object?>>> SortMap = new()
    {
        [TagSortBy.Name] = t => t.Name,
        [TagSortBy.CreatedAt] = t => t.CreatedAt,
        [TagSortBy.UpdatedAt] = t => t.UpdatedAt,
    };

    public static IQueryable<Tag> ApplyFilter(this IQueryable<Tag> q, TagFilterParam f)
    {
        return q.WhereIf(f.Name is not null, t => t.Name.Contains(f.Name!))
            .WhereIf(f.CreatedAtFrom.HasValue, t => t.CreatedAt >= f.CreatedAtFrom)
            .WhereIf(f.CreatedAtTo.HasValue, t => t.CreatedAt <= f.CreatedAtTo)
            .WhereIf(f.UpdatedAtFrom.HasValue, t => t.UpdatedAt >= f.UpdatedAtFrom)
            .WhereIf(f.UpdatedAtTo.HasValue, t => t.UpdatedAt <= f.UpdatedAtTo)
            .WhereIf(f.IsDeleted.HasValue, t => t.IsDeleted == f.IsDeleted);
    }

    public static IQueryable<Tag> ApplySort(this IQueryable<Tag> q, TagFilterParam f)
    {
        var keySelector = SortMap[f.SortBy];

        var ordered =
            f.SortOrder == SortOrder.Descending
                ? q.OrderByDescending(keySelector)
                : q.OrderBy(keySelector);

        return ordered.ThenByDescending(t => t.Id);
    }
}
