using Bookstore.Api.Common.Enums;
using Bookstore.Api.Common.Extensions;
using Bookstore.Api.DTOs.Series;
using Bookstore.Data.Entities;

namespace Bookstore.Api.Queries;

public static class SerieQueryExtenstion
{
    public static IQueryable<Serie> ApplyBaseFilter(this IQueryable<Serie> q, SerieFilterParam f)
    {
        return q.WhereIf(f.Name is not null, t => t.Name.Contains(f.Name!));
    }

    public static IQueryable<Serie> ApplyManagementFilter(
        this IQueryable<Serie> q,
        SerieManagementFilterParam f
    )
    {
        return q.ApplyBaseFilter(f)
            .WhereIf(f.CreatedAtFrom.HasValue, t => t.CreatedAt >= f.CreatedAtFrom)
            .WhereIf(f.CreatedAtTo.HasValue, t => t.CreatedAt <= f.CreatedAtTo)
            .WhereIf(f.UpdatedAtFrom.HasValue, t => t.UpdatedAt >= f.UpdatedAtFrom)
            .WhereIf(f.UpdatedAtTo.HasValue, t => t.UpdatedAt <= f.UpdatedAtTo)
            .WhereIf(f.IsDeleted.HasValue, t => t.IsDeleted == f.IsDeleted);
    }

    public static IQueryable<Serie> ApplyManagementSort(
        this IQueryable<Serie> q,
        SerieManagementFilterParam f
    )
    {
        var isDescending = f.SortOrder == SortOrder.Descending;

        var ordered = f.SortBy switch
        {
            SerieManagementSortBy.Name => isDescending
                ? q.OrderByDescending(t => t.Name)
                : q.OrderBy(t => t.Name),
            SerieManagementSortBy.CreatedAt => isDescending
                ? q.OrderByDescending(t => t.CreatedAt)
                : q.OrderBy(t => t.CreatedAt),
            SerieManagementSortBy.UpdatedAt => isDescending
                ? q.OrderByDescending(t => t.UpdatedAt)
                : q.OrderBy(t => t.UpdatedAt),
            _ => throw new ArgumentOutOfRangeException($"Invalid sort by value: {f.SortBy}"),
        };

        return ordered.ThenByDescending(t => t.Id);
    }
}
