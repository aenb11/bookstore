using Bookstore.Api.Common.Enums;

namespace Bookstore.Api.DTOs.Publishers;

public enum PublisherManagementSortBy
{
    Name,
    CreatedAt,
    UpdatedAt,
}

public class PublisherManagementFilterParam : PublisherFilterParam
{
    public DateTimeOffset? CreatedFrom { get; set; }
    public DateTimeOffset? CreatedTo { get; set; }

    public DateTimeOffset? UpdatedFrom { get; set; }
    public DateTimeOffset? UpdatedTo { get; set; }

    public bool? IsDeleted { get; set; }

    public PublisherManagementSortBy SortBy { get; set; } = PublisherManagementSortBy.Name;
    public SortOrder SortOrder { get; set; } = SortOrder.Ascending;
}
