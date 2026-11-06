using System.ComponentModel.DataAnnotations;
using Bookstore.Api.Common.Enums;
using Bookstore.Api.Common.Pagination;

namespace Bookstore.Api.DTOs.Reviews;

public enum ReviewSortBy
{
    CreatedAt,
    Rating,
}

public class ReviewFilterParam : PaginationParam, IValidatableObject
{
    [Range(1, int.MaxValue, ErrorMessage = "BookId must be a positive integer.")]
    public int? BookId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "AccountId must be a positive integer.")]
    public int? AccountId { get; set; }

    [Range(1, 5, ErrorMessage = "MinRating must be between 1 and 5.")]
    public int? MinRating { get; set; }

    [Range(1, 5, ErrorMessage = "MaxRating must be between 1 and 5.")]
    public int? MaxRating { get; set; }

    public DateTimeOffset? FromDate { get; set; }
    public DateTimeOffset? ToDate { get; set; }
    public bool? IsDeleted { get; set; }
    public ReviewSortBy SortBy { get; set; } = ReviewSortBy.CreatedAt;
    public SortOrder SortOrder { get; set; } = SortOrder.Descending;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (MinRating > MaxRating)
        {
            yield return new ValidationResult(
                "MinRating cannot be greater than MaxRating.",
                [nameof(MinRating), nameof(MaxRating)]
            );
        }

        if (FromDate > ToDate)
        {
            yield return new ValidationResult(
                "FromDate cannot be later than ToDate.",
                [nameof(FromDate), nameof(ToDate)]
            );
        }
    }
}
