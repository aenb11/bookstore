using System.Linq.Expressions;
using Bookstore.Api.DTOs.Books;
using Bookstore.Data.Entities;

namespace Bookstore.Api.Common.Projections;

public static class BookProjection
{
    public static readonly Expression<Func<Book, BookSummaryResponseDto>> ToSummaryResponseDto =
        b => new BookSummaryResponseDto
        {
            Id = b.Id,
            ThumbnailUrl =
                b.Images.OrderBy(i => i.SortOrder).Select(i => i.ImageKey).FirstOrDefault() ?? "",
            Title = b.Title,
            ListPrice = b.ListPrice,
            SalePrice = b.SalePrice,
            StockQuantity = b.StockQuantity,
            TotalReviews = b.TotalReviews,
            AverageRating = b.AverageRating,
            SoldCount = b.SoldCount,
        };

    public static readonly Expression<Func<Book, BookResponseDto>> ToResponseDto =
        b => new BookResponseDto
        {
            Id = b.Id,
            Isbn = b.Isbn,
            Title = b.Title,

            Category = new BookResponseDto.CategoryItem(b.Category.Id, b.Category.Name),
            Serie =
                b.Serie != null
                    ? new BookResponseDto.SerieItem(b.Serie.Id, b.Serie.Name, b.Serie.Description)
                    : null,
            Publisher = new BookResponseDto.PublisherItem(b.Publisher.Id, b.Publisher.Name),
            PublicationYear = b.PublicationYear,
            Language = b.Language,
            PageCount = b.PageCount,
            CoverType = b.CoverType,
            Weight = b.Weight,
            AgeGroup = b.AgeGroup,
            ListPrice = b.ListPrice,
            SalePrice = b.SalePrice,
            StockQuantity = b.StockQuantity,
            Description = b.Description,

            TotalReviews = b.TotalReviews,
            AverageRating = b.AverageRating,
            SoldCount = b.SoldCount,

            Authors = b
                .BookAuthors.Select(ba => new BookResponseDto.AuthorItem(
                    ba.Author.Id,
                    ba.Author.FullName,
                    ba.Role
                ))
                .ToList(),
            Images = b
                .Images.Select(i => new BookResponseDto.ImageItem(i.ImageKey, i.SortOrder))
                .ToList(),
            Tags = b.Tags.Select(t => new BookResponseDto.TagItem(t.Name)).ToList(),
        };

    public static readonly Expression<
        Func<Book, BookDetailSummaryResponseDto>
    > ToDetailSummaryResponseDto = b => new BookDetailSummaryResponseDto
    {
        Id = b.Id,
        ThumbnailUrl =
            b.Images.OrderBy(i => i.SortOrder).Select(i => i.ImageKey).FirstOrDefault() ?? "",
        Title = b.Title,
        ListPrice = b.ListPrice,
        SalePrice = b.SalePrice,
        StockQuantity = b.StockQuantity,
        Status = b.Status,
        TotalReviews = b.TotalReviews,
        AverageRating = b.AverageRating,
        SoldCount = b.SoldCount,
        CreatedAt = b.CreatedAt,
        UpdatedAt = b.UpdatedAt,
        IsDeleted = b.IsDeleted,
        DeletedAt = b.DeletedAt,
    };

    public static readonly Expression<Func<Book, BookDetailResponseDto>> ToDetailResponseDto =
        b => new BookDetailResponseDto
        {
            Id = b.Id,
            Isbn = b.Isbn,
            Title = b.Title,

            Category = new BookDetailResponseDto.CategoryItem(b.Category.Id, b.Category.Name),
            Serie =
                b.Serie != null
                    ? new BookDetailResponseDto.SerieItem(
                        b.Serie.Id,
                        b.Serie.Name,
                        b.Serie.Description
                    )
                    : null,
            Publisher = new BookDetailResponseDto.PublisherItem(b.Publisher.Id, b.Publisher.Name),
            PublicationYear = b.PublicationYear,
            Language = b.Language,
            PageCount = b.PageCount,
            CoverType = b.CoverType,
            Weight = b.Weight,
            AgeGroup = b.AgeGroup,
            ListPrice = b.ListPrice,
            SalePrice = b.SalePrice,
            StockQuantity = b.StockQuantity,
            Status = b.Status,
            Description = b.Description,

            TotalReviews = b.TotalReviews,
            AverageRating = b.AverageRating,
            SoldCount = b.SoldCount,

            Authors = b
                .BookAuthors.Select(ba => new BookDetailResponseDto.AuthorItem(
                    ba.Author.Id,
                    ba.Author.FullName,
                    ba.Role
                ))
                .ToList(),
            Images = b
                .Images.Select(i => new BookDetailResponseDto.ImageItem(i.ImageKey, i.SortOrder))
                .ToList(),
            Tags = b.Tags.Select(t => new BookDetailResponseDto.TagItem(t.Name)).ToList(),

            CreatedAt = b.CreatedAt,
            UpdatedAt = b.UpdatedAt,
            IsDeleted = b.IsDeleted,
            DeletedAt = b.DeletedAt,
        };

    public static readonly Func<Book, BookDetailResponseDto> CompiledToDetailResponseDto =
        ToDetailResponseDto.Compile();
}
