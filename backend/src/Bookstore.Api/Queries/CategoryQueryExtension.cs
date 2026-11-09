using Bookstore.Api.Common.Enums;
using Bookstore.Api.Common.Extensions;
using Bookstore.Api.DTOs.Categories;
using Bookstore.Data.Entities;

namespace Bookstore.Api.Queries;

public static class CategoryQueryExtension
{
    public static IQueryable<Category> ApplyBaseFilter(
        this IQueryable<Category> q,
        CategoryFilterParam f
    )
    {
        return q.WhereIf(f.Name is not null, t => t.Name.Contains(f.Name!));
    }

    public static IQueryable<Category> ApplyManagementFilter(
        this IQueryable<Category> q,
        CategoryManagementFilterParam f
    )
    {
        return q.ApplyBaseFilter(f)
            .WhereIf(f.CreatedAtFrom.HasValue, t => t.CreatedAt >= f.CreatedAtFrom)
            .WhereIf(f.CreatedAtTo.HasValue, t => t.CreatedAt <= f.CreatedAtTo)
            .WhereIf(f.UpdatedAtFrom.HasValue, t => t.UpdatedAt >= f.UpdatedAtFrom)
            .WhereIf(f.UpdatedAtTo.HasValue, t => t.UpdatedAt <= f.UpdatedAtTo)
            .WhereIf(f.IsDeleted.HasValue, t => t.IsDeleted == f.IsDeleted);
    }

    public static IQueryable<Category> ApplyManagementSort(
        this IQueryable<Category> q,
        CategoryManagementFilterParam f
    )
    {
        var isDescending = f.SortOrder == SortOrder.Descending;
        var ordered = f.SortBy switch
        {
            CategoryManagementSortBy.Name => isDescending
                ? q.OrderByDescending(t => t.Name)
                : q.OrderBy(t => t.Name),
            CategoryManagementSortBy.CreatedAt => isDescending
                ? q.OrderByDescending(t => t.CreatedAt)
                : q.OrderBy(t => t.CreatedAt),
            CategoryManagementSortBy.UpdatedAt => isDescending
                ? q.OrderByDescending(t => t.UpdatedAt)
                : q.OrderBy(t => t.UpdatedAt),
            _ => throw new ArgumentOutOfRangeException($"Invalid sort by value: {f.SortBy}"),
        };

        return ordered.ThenByDescending(t => t.Id);
    }
}
