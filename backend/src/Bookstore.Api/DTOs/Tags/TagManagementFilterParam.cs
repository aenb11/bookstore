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
    public DateTimeOffset? CreatedFrom { get; set; }
    public DateTimeOffset? CreatedTo { get; set; }

    public DateTimeOffset? UpdatedFrom { get; set; }
    public DateTimeOffset? UpdatedTo { get; set; }

    public bool? IsDeleted { get; set; }

    public TagManagementSortBy SortBy { get; set; } = TagManagementSortBy.Name;
    public SortOrder SortOrder { get; set; } = SortOrder.Ascending;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (CreatedFrom > CreatedTo)
        {
            yield return new ValidationResult(
                "CreatedAtFrom cannot be later than CreatedAtTo.",
                [nameof(CreatedFrom), nameof(CreatedTo)]
            );
        }

        if (UpdatedFrom > UpdatedTo)
        {
            yield return new ValidationResult(
                "UpdatedAtFrom cannot be later than UpdatedAtTo.",
                [nameof(UpdatedFrom), nameof(UpdatedTo)]
            );
        }
    }
}
