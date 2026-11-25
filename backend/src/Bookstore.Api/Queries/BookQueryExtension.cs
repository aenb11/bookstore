using Bookstore.Api.Common.Enums;
using Bookstore.Api.Common.Extensions;
using Bookstore.Api.DTOs.Books;
using Bookstore.Data.Entities;

namespace Bookstore.Api.Queries;

public static class BookQueryExtension
{
    public static IQueryable<Book> ApplyBaseFilter<TSortBy>(
        this IQueryable<Book> q,
        BookFilterParamBase<TSortBy> f
    )
        where TSortBy : struct, Enum
    {
        return q.WhereIf(f.Keyword is not null, x => x.Title.Contains(f.Keyword!))
            .WhereIf(
                f.Keyword is not null,
                x => x.Authors.Any(a => a.FullName.Contains(f.Keyword!))
            )
            .WhereIf(
                !string.IsNullOrWhiteSpace(f.Keyword),
                x => x.Isbn != null && x.Isbn.Contains(f.Keyword!)
            )
            .WhereIf(f.CategoryId.HasValue, x => x.CategoryId == f.CategoryId)
            .WhereIf(f.SerieId.HasValue, x => x.SerieId == f.SerieId)
            .WhereIf(f.PublisherId.HasValue, x => x.PublisherId == f.PublisherId)
            .WhereIf(
                f.PublicationYearFrom.HasValue,
                x => x.PublicationYear >= f.PublicationYearFrom
            )
            .WhereIf(f.PublicationYearTo.HasValue, x => x.PublicationYear <= f.PublicationYearTo)
            .WhereIf(f.PriceFrom.HasValue, x => x.SalePrice >= f.PriceFrom)
            .WhereIf(f.PriceTo.HasValue, x => x.SalePrice <= f.PriceTo)
            .WhereIf(f.AverageRatingFrom.HasValue, x => x.AverageRating >= f.AverageRatingFrom)
            .WhereIf(
                f.IsOnStock.HasValue,
                x => f.IsOnStock!.Value ? x.StockQuantity > 0 : x.StockQuantity == 0
            )
            .WhereIf(
                f.IsDiscounted.HasValue,
                x => f.IsDiscounted!.Value ? x.SalePrice < x.ListPrice : x.SalePrice == x.ListPrice
            );
    }

    public static IQueryable<Book> ApplyManagementFilter(
        this IQueryable<Book> q,
        BookManagementFilterParam f
    )
    {
        return q.ApplyBaseFilter(f)
            .WhereIf(f.Status.HasValue, x => x.Status == f.Status)
            .WhereIf(f.IsDeleted.HasValue, x => x.IsDeleted == f.IsDeleted);
    }

    public static IQueryable<Book> ApplyBaseSort(this IQueryable<Book> q, BookFilterParam f)
    {
        var isDescending = f.SortOrder == SortOrder.Descending;

        var baseOrdered = q.OrderByDescending(t => t.StockQuantity > 0);

        var ordered = f.SortBy switch
        {
            BookSortBy.Title => isDescending
                ? baseOrdered.ThenByDescending(t => t.Title)
                : baseOrdered.ThenBy(t => t.Title),
            BookSortBy.Price => isDescending
                ? baseOrdered.ThenByDescending(t => t.SalePrice)
                : baseOrdered.ThenBy(t => t.SalePrice),
            BookSortBy.Rating => isDescending
                ? baseOrdered.ThenByDescending(t => t.AverageRating)
                : baseOrdered.ThenBy(t => t.AverageRating),
            BookSortBy.SoldCount => isDescending
                ? baseOrdered.ThenByDescending(t => t.SoldCount)
                : baseOrdered.ThenBy(t => t.SoldCount),
            BookSortBy.PublicationYear => isDescending
                ? baseOrdered.ThenByDescending(t => t.PublicationYear)
                : baseOrdered.ThenBy(t => t.PublicationYear),
            BookSortBy.CreatedAt => isDescending
                ? baseOrdered.ThenByDescending(t => t.CreatedAt)
                : baseOrdered.ThenBy(t => t.CreatedAt),
            _ => throw new ArgumentOutOfRangeException($"Invalid sort by value: {f.SortBy}"),
        };

        return ordered.ThenByDescending(t => t.Id);
    }

    public static IQueryable<Book> ApplyManagementSort(
        this IQueryable<Book> q,
        BookManagementFilterParam f
    )
    {
        var isDescending = f.SortOrder == SortOrder.Descending;

        var ordered = f.SortBy switch
        {
            BookManagementSortBy.Title => isDescending
                ? q.OrderByDescending(t => t.Title)
                : q.OrderBy(t => t.Title),
            BookManagementSortBy.Price => isDescending
                ? q.OrderByDescending(t => t.SalePrice)
                : q.OrderBy(t => t.SalePrice),
            BookManagementSortBy.Rating => isDescending
                ? q.OrderByDescending(t => t.AverageRating)
                : q.OrderBy(t => t.AverageRating),
            BookManagementSortBy.SoldCount => isDescending
                ? q.OrderByDescending(t => t.SoldCount)
                : q.OrderBy(t => t.SoldCount),
            BookManagementSortBy.PublicationYear => isDescending
                ? q.OrderByDescending(t => t.PublicationYear)
                : q.OrderBy(t => t.PublicationYear),
            BookManagementSortBy.StockQuantity => isDescending
                ? q.OrderByDescending(t => t.StockQuantity)
                : q.OrderBy(t => t.StockQuantity),
            BookManagementSortBy.CreatedAt => isDescending
                ? q.OrderByDescending(t => t.CreatedAt)
                : q.OrderBy(t => t.CreatedAt),
            BookManagementSortBy.UpdatedAt => isDescending
                ? q.OrderByDescending(t => t.UpdatedAt)
                : q.OrderBy(t => t.UpdatedAt),
            _ => throw new ArgumentOutOfRangeException($"Invalid sort by value: {f.SortBy}"),
        };

        return ordered.ThenByDescending(t => t.Id);
    }
}
