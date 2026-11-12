using System.ComponentModel.DataAnnotations;
using Bookstore.Api.Common.Enums;

namespace Bookstore.Api.DTOs.Publishers;

public enum PublisherManagementSortBy
{
    Name,
    CreatedAt,
    UpdatedAt,
}

public class PublisherManagementFilterParam : PublisherFilterParam, IValidatableObject
{
    public DateTimeOffset? CreatedFrom { get; set; }
    public DateTimeOffset? CreatedTo { get; set; }

    public DateTimeOffset? UpdatedFrom { get; set; }
    public DateTimeOffset? UpdatedTo { get; set; }

    public bool? IsDeleted { get; set; }

    public PublisherManagementSortBy SortBy { get; set; } = PublisherManagementSortBy.Name;
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
