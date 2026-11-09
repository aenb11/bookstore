using Bookstore.Api.Common.Enums;
using Bookstore.Api.Common.Extensions;
using Bookstore.Api.DTOs.Reviews;
using Bookstore.Data.Entities;

namespace Bookstore.Api.Queries;

public static class ReviewQueryExtension
{
    public static IQueryable<Review> ApplyBaseFilter(this IQueryable<Review> q, ReviewFilterParam f)
    {
        return q.WhereIf(f.MinRating.HasValue, r => r.Rating >= f.MinRating)
            .WhereIf(f.MaxRating.HasValue, r => r.Rating <= f.MaxRating)
            .WhereIf(f.FromDate.HasValue, r => r.CreatedAt >= f.FromDate)
            .WhereIf(f.ToDate.HasValue, r => r.CreatedAt <= f.ToDate);
    }

    public static IQueryable<Review> ApplyManagementFilter(
        this IQueryable<Review> q,
        ReviewManagementFilterParam f
    )
    {
        return q.ApplyBaseFilter(f)
            .WhereIf(f.BookId.HasValue, r => r.BookId == f.BookId)
            .WhereIf(f.AccountId.HasValue, r => r.AccountId == f.AccountId)
            .WhereIf(f.IsDeleted.HasValue, r => r.IsDeleted == f.IsDeleted);
    }

    public static IQueryable<Review> ApplySort(this IQueryable<Review> q, ReviewFilterParam f)
    {
        var isDescending = f.SortOrder == SortOrder.Descending;
        var ordered = f.SortBy switch
        {
            ReviewSortBy.Rating => isDescending
                ? q.OrderByDescending(r => r.Rating)
                : q.OrderBy(r => r.Rating),
            ReviewSortBy.CreatedAt => isDescending
                ? q.OrderByDescending(r => r.CreatedAt)
                : q.OrderBy(r => r.CreatedAt),
            _ => throw new ArgumentOutOfRangeException($"Invalid sort by value: {f.SortBy}"),
        };

        return ordered.ThenByDescending(r => r.Id);
    }
}
