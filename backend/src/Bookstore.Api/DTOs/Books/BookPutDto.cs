using System.ComponentModel.DataAnnotations;
using Bookstore.Data.Entities;

namespace Bookstore.Api.DTOs.Books;

public class BookPutDto : IValidatableObject
{
    [StringLength(
        13,
        MinimumLength = 13,
        ErrorMessage = "ISBN must be exactly 13 characters long."
    )]
    public string? Isbn { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
    public string Title { get; set; } = null!;

    [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be a positive integer.")]
    public int CategoryId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "SerieId must be a positive integer.")]
    public int? SerieId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "PublisherId must be a positive integer.")]
    public int PublisherId { get; set; }

    [Range(0, 2100, ErrorMessage = "PublicationYear must be between 0 and 2100.")]
    public int PublicationYear { get; set; }

    [Required(ErrorMessage = "Language is required.")]
    [StringLength(
        2,
        MinimumLength = 2,
        ErrorMessage = "Language must be exactly 2 characters long."
    )]
    public string Language { get; set; } = null!;

    [Range(1, int.MaxValue, ErrorMessage = "PageCount must be a positive integer.")]
    public int PageCount { get; set; }

    [EnumDataType(typeof(BookCover), ErrorMessage = "Invalid CoverType.")]
    public BookCover CoverType { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Weight must be a non-negative integer.")]
    public int Weight { get; set; }

    [EnumDataType(typeof(BookAgeGroup), ErrorMessage = "Invalid AgeGroup.")]
    public BookAgeGroup AgeGroup { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "ListPrice must be a non-negative integer.")]
    public int ListPrice { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "SalePrice must be a non-negative integer.")]
    public int SalePrice { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "StockQuantity must be a non-negative integer.")]
    public int StockQuantity { get; set; }

    [EnumDataType(typeof(BookStatus), ErrorMessage = "Invalid Status.")]
    public BookStatus Status { get; set; }

    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
    public string? Description { get; set; }

    public IReadOnlyCollection<int> TagIds { get; set; } = [];

    [MinLength(1, ErrorMessage = "At least 1 author is required.")]
    public IReadOnlyCollection<AuthorItem> Authors { get; set; } = [];

    [MinLength(1, ErrorMessage = "At least 1 image is required.")]
    [MaxLength(5, ErrorMessage = "You can upload at most 5 images.")]
    public IReadOnlyCollection<BookImageUpdateItem> Images { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (SalePrice > ListPrice)
            yield return new ValidationResult(
                "SalePrice cannot be greater than ListPrice.",
                [nameof(SalePrice)]
            );

        if (Authors.Select(a => a.Id).Distinct().Count() != Authors.Count)
            yield return new ValidationResult(
                "Duplicate author IDs are not allowed.",
                [nameof(Authors)]
            );

        if (Images.Select(i => i.ImageKey).Distinct().Count() != Images.Count)
            yield return new ValidationResult(
                "Duplicate image keys are not allowed.",
                [nameof(Images)]
            );

        var existingIds = Images.Where(i => i.Id.HasValue).Select(i => i.Id!.Value).ToList();
        if (existingIds.Distinct().Count() != existingIds.Count)
            yield return new ValidationResult(
                "Duplicate image IDs are not allowed.",
                [nameof(Images)]
            );
    }

    public record AuthorItem(
        [property: Range(1, int.MaxValue, ErrorMessage = "Author Id must be a positive integer.")]
            int Id,
        [property: EnumDataType(typeof(BookAuthorRole), ErrorMessage = "Invalid author role.")]
            BookAuthorRole Role
    );

    public record BookImageUpdateItem(
        [property: Range(1, int.MaxValue, ErrorMessage = "Image Id must be a positive integer.")]
            int? Id,
        [property: Required(ErrorMessage = "ImageKey is required.")]
        [property: StringLength(255, ErrorMessage = "ImageKey cannot exceed 255 characters.")]
            string ImageKey,
        [property: Range(0, 100, ErrorMessage = "SortOrder must be between 0 and 100.")]
            int SortOrder
    );
}
