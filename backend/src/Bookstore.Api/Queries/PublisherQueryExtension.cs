using Bookstore.Api.Common.Enums;
using Bookstore.Api.Common.Extensions;
using Bookstore.Api.DTOs.Publishers;
using Bookstore.Data.Entities;

namespace Bookstore.Api.Queries;

public static class PublisherQueryExtension
{
    public static IQueryable<Publisher> ApplyBaseFilter(
        this IQueryable<Publisher> q,
        PublisherFilterParam f
    )
    {
        return q.WhereIf(f.Name is not null, p => p.Name.Contains(f.Name!));
    }

    public static IQueryable<Publisher> ApplyManagementFilter(
        this IQueryable<Publisher> q,
        PublisherManagementFilterParam f
    )
    {
        return q.ApplyBaseFilter(f)
            .WhereIf(f.CreatedAtFrom.HasValue, p => p.CreatedAt >= f.CreatedAtFrom)
            .WhereIf(f.CreatedAtTo.HasValue, p => p.CreatedAt <= f.CreatedAtTo)
            .WhereIf(f.UpdatedAtFrom.HasValue, p => p.UpdatedAt >= f.UpdatedAtFrom)
            .WhereIf(f.UpdatedAtTo.HasValue, p => p.UpdatedAt <= f.UpdatedAtTo)
            .WhereIf(f.IsDeleted.HasValue, p => p.IsDeleted == f.IsDeleted);
    }

    public static IQueryable<Publisher> ApplyManagementSort(
        this IQueryable<Publisher> q,
        PublisherManagementFilterParam f
    )
    {
        var isDescending = f.SortOrder == SortOrder.Descending;

        var ordered = f.SortBy switch
        {
            PublisherManagementSortBy.Name => isDescending
                ? q.OrderByDescending(p => p.Name)
                : q.OrderBy(p => p.Name),
            PublisherManagementSortBy.CreatedAt => isDescending
                ? q.OrderByDescending(p => p.CreatedAt)
                : q.OrderBy(p => p.CreatedAt),
            PublisherManagementSortBy.UpdatedAt => isDescending
                ? q.OrderByDescending(p => p.UpdatedAt)
                : q.OrderBy(p => p.UpdatedAt),
            _ => throw new ArgumentOutOfRangeException($"Invalid sort by value: {f.SortBy}"),
        };

        return ordered.ThenByDescending(p => p.Id);
    }
}
