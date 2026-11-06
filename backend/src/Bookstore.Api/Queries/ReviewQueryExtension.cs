using System.Linq.Expressions;
using Bookstore.Api.Common.Extensions;
using Bookstore.Api.DTOs.Reviews;
using Bookstore.Data.Entities;

namespace Bookstore.Api.Queries;

public static class ReviewQueryExtension
{
    private static readonly Dictionary<ReviewSortBy, Expression<Func<Review, object>>> SortMap =
        new()
        {
            [ReviewSortBy.CreatedAt] = r => r.CreatedAt,
            [ReviewSortBy.Rating] = r => r.Rating,
        };

    public static IQueryable<Review> ApplyFilter(this IQueryable<Review> q, ReviewFilterParam f)
    {
        return q.WhereIf(f.BookId.HasValue, r => r.BookId == f.BookId)
            .WhereIf(f.AccountId.HasValue, r => r.AccountId == f.AccountId)
            .WhereIf(f.MinRating.HasValue, r => r.Rating >= f.MinRating)
            .WhereIf(f.MaxRating.HasValue, r => r.Rating <= f.MaxRating)
            .WhereIf(f.FromDate.HasValue, r => r.CreatedAt >= f.FromDate)
            .WhereIf(f.ToDate.HasValue, r => r.CreatedAt <= f.ToDate)
            .WhereIf(f.IsDeleted.HasValue, r => r.IsDeleted == f.IsDeleted);
    }

    public static IQueryable<Review> ApplySort(this IQueryable<Review> q, ReviewFilterParam f)
    {
        var keySelector = SortMap[f.SortBy];

        var ordered =
            f.SortOrder == ReviewSortOrder.Desc
                ? q.OrderByDescending(keySelector)
                : q.OrderBy(keySelector);

        return ordered.ThenByDescending(r => r.Id);
    }
}
