using Bookstore.Api.Common.Enums;
using Bookstore.Api.Common.Extensions;
using Bookstore.Api.DTOs.Tags;
using Bookstore.Data.Entities;

namespace Bookstore.Api.Queries;

public static class TagQueryExtenstion
{
    public static IQueryable<Tag> ApplyBaseFilter(this IQueryable<Tag> q, TagFilterParam f)
    {
        return q.WhereIf(f.Name is not null, t => t.Name.Contains(f.Name!));
    }

    public static IQueryable<Tag> ApplyManagementFilter(
        this IQueryable<Tag> q,
        TagManagementFilterParam f
    )
    {
        return q.ApplyBaseFilter(f)
            .WhereIf(f.CreatedAtFrom.HasValue, t => t.CreatedAt >= f.CreatedAtFrom)
            .WhereIf(f.CreatedAtTo.HasValue, t => t.CreatedAt <= f.CreatedAtTo)
            .WhereIf(f.UpdatedAtFrom.HasValue, t => t.UpdatedAt >= f.UpdatedAtFrom)
            .WhereIf(f.UpdatedAtTo.HasValue, t => t.UpdatedAt <= f.UpdatedAtTo)
            .WhereIf(f.IsDeleted.HasValue, t => t.IsDeleted == f.IsDeleted);
    }

    public static IQueryable<Tag> ApplyManagementSort(
        this IQueryable<Tag> q,
        TagManagementFilterParam f
    )
    {
        var isDescending = f.SortOrder == SortOrder.Descending;

        var ordered = f.SortBy switch
        {
            TagManagementSortBy.Name => isDescending
                ? q.OrderByDescending(t => t.Name)
                : q.OrderBy(t => t.Name),
            TagManagementSortBy.CreatedAt => isDescending
                ? q.OrderByDescending(t => t.CreatedAt)
                : q.OrderBy(t => t.CreatedAt),
            TagManagementSortBy.UpdatedAt => isDescending
                ? q.OrderByDescending(t => t.UpdatedAt)
                : q.OrderBy(t => t.UpdatedAt),
            _ => throw new ArgumentOutOfRangeException($"Invalid sort by value: {f.SortBy}"),
        };

        return ordered.ThenByDescending(t => t.Id);
    }
}
