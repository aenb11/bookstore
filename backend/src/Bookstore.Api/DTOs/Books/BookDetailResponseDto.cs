using Bookstore.Data.Entities;

namespace Bookstore.Api.DTOs.Books;

public class BookDetailResponseDto
{
    public int Id { get; set; }
    public string? Isbn { get; set; }
    public string Title { get; set; } = null!;

    public CategoryItem Category { get; set; } = null!;
    public SerieItem? Serie { get; set; }
    public PublisherItem Publisher { get; set; } = null!;
    public int PublicationYear { get; set; }
    public string Language { get; set; } = null!;
    public int PageCount { get; set; }
    public BookCover CoverType { get; set; }
    public int Weight { get; set; }
    public BookAgeGroup AgeGroup { get; set; }
    public int ListPrice { get; set; }
    public int SalePrice { get; set; }
    public int StockQuantity { get; set; }
    public BookStatus Status { get; set; }
    public string? Description { get; set; }

    public int TotalReviews { get; set; }
    public double AverageRating { get; set; }
    public int SoldCount { get; set; }

    public List<AuthorItem> Authors { get; set; } = [];
    public List<ImageItem> Images { get; set; } = [];
    public List<TagItem> Tags { get; set; } = [];

    public record CategoryItem(int Id, string Name);

    public record SerieItem(int Id, string Name, string? Description);

    public record PublisherItem(int Id, string Name);

    public record AuthorItem(int Id, string FullName, BookAuthorRole Role);

    public record ImageItem(string Url, int SortOrder);

    public record TagItem(string Name);

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}
