using Bookstore.Api.Common.Enums;

namespace Bookstore.Api.DTOs.Tags;

public enum TagManagementSortBy
{
    Name,
    CreatedAt,
    UpdatedAt,
}

public class TagManagementFilterParam : TagFilterParam
{
    public DateTimeOffset? CreatedFrom { get; set; }
    public DateTimeOffset? CreatedTo { get; set; }

    public DateTimeOffset? UpdatedFrom { get; set; }
    public DateTimeOffset? UpdatedTo { get; set; }

    public bool? IsDeleted { get; set; }

    public TagManagementSortBy SortBy { get; set; } = TagManagementSortBy.Name;
    public SortOrder SortOrder { get; set; } = SortOrder.Ascending;
}
