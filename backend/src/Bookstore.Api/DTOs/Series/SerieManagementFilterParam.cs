using System.ComponentModel.DataAnnotations;
using Bookstore.Api.Common.Enums;

namespace Bookstore.Api.DTOs.Series;

public enum SerieManagementSortBy
{
    Name,
    CreatedAt,
    UpdatedAt,
}

public class SerieManagementFilterParam : SerieFilterParam, IValidatableObject
{
    public DateTimeOffset? CreatedAtFrom { get; set; }
    public DateTimeOffset? CreatedAtTo { get; set; }

    public DateTimeOffset? UpdatedAtFrom { get; set; }
    public DateTimeOffset? UpdatedAtTo { get; set; }

    public bool? IsDeleted { get; set; }

    public SerieManagementSortBy SortBy { get; set; } = SerieManagementSortBy.Name;
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
