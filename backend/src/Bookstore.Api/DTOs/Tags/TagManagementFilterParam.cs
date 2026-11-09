using System.ComponentModel.DataAnnotations;
using Bookstore.Api.Common.Enums;

namespace Bookstore.Api.DTOs.Tags;

public enum TagManagementSortBy
{
    Name,
    CreatedAt,
    UpdatedAt,
}

public class TagManagementFilterParam : TagFilterParam, IValidatableObject
{
    public DateTimeOffset? CreatedAtFrom { get; set; }
    public DateTimeOffset? CreatedAtTo { get; set; }

    public DateTimeOffset? UpdatedAtFrom { get; set; }
    public DateTimeOffset? UpdatedAtTo { get; set; }

    public bool? IsDeleted { get; set; }

    public TagManagementSortBy SortBy { get; set; } = TagManagementSortBy.Name;
    public SortOrder SortOrder { get; set; } = SortOrder.Ascending;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (CreatedAtFrom > CreatedAtTo)
        {
            yield return new ValidationResult(
                "CreatedAtFrom cannot be later than CreatedAtTo.",
                [nameof(CreatedAtFrom), nameof(CreatedAtTo)]
            );
        }

        if (UpdatedAtFrom > UpdatedAtTo)
        {
            yield return new ValidationResult(
                "UpdatedAtFrom cannot be later than UpdatedAtTo.",
                [nameof(UpdatedAtFrom), nameof(UpdatedAtTo)]
            );
        }
    }
}
