using Bookstore.Api.Common.Enums;

namespace Bookstore.Api.DTOs.Series;

public enum SerieManagementSortBy
{
    Name,
    CreatedAt,
    UpdatedAt,
}

public class SerieManagementFilterParam : SerieFilterParam
{
    public DateTimeOffset? CreatedFrom { get; set; }
    public DateTimeOffset? CreatedTo { get; set; }

    public DateTimeOffset? UpdatedFrom { get; set; }
    public DateTimeOffset? UpdatedTo { get; set; }

    public bool? IsDeleted { get; set; }

    public SerieManagementSortBy SortBy { get; set; } = SerieManagementSortBy.Name;
    public SortOrder SortOrder { get; set; } = SortOrder.Ascending;
}
