using Bookstore.Data.Common;

namespace Bookstore.Data.Entities;

public enum BookCover
{
    Hardcover,
    Paperback,
}

public enum BookStatus
{
    Draft,
    Available,
    Archived,
    Discontinued,
}

public enum BookAgeGroup
{
    Everyone,
    Children,
    YoungAdult,
    Adult,
}

public class Book : IAuditableEntity, ISoftDeletableEntity
{
    public int Id { get; init; }
    public string? Isbn { get; set; }
    public required string Title { get; set; }

    public required int CategoryId { get; set; }
    public int? SerieId { get; set; }
    public required int PublisherId { get; set; }
    public required int PublicationYear { get; set; }
    public required string Language { get; set; }
    public required int PageCount { get; set; }
    public required BookCover CoverType { get; set; }
    public required int Weight { get; set; }
    public required BookAgeGroup AgeGroup { get; set; }
    public required int ListPrice { get; set; }
    public required int SalePrice { get; set; }
    public int StockQuantity { get; set; } = 0;
    public BookStatus Status { get; set; } = BookStatus.Available;
    public string? Description { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTimeOffset? DeletedAt { get; set; }

    public Category Category { get; set; } = null!;
    public Publisher Publisher { get; set; } = null!;
    public Serie? Serie { get; set; }

    public ICollection<Author> Authors { get; set; } = [];
    public ICollection<BookImage> Images { get; set; } = [];
    public ICollection<Tag> Tags { get; set; } = [];
}
