using Bookstore.Api.Common.Enums;

namespace Bookstore.Api.DTOs.Authors;

public enum AuthorManagementSortBy
{
    Name,
    CreatedAt,
    UpdatedAt,
}

public class AuthorManagementFilterParam : AuthorFilterParam
{
    public DateTimeOffset? CreatedFrom { get; set; }
    public DateTimeOffset? CreatedTo { get; set; }
    public DateTimeOffset? UpdatedFrom { get; set; }
    public DateTimeOffset? UpdatedTo { get; set; }
    public bool? IsDeleted { get; set; }

    public AuthorManagementSortBy SortBy { get; set; } = AuthorManagementSortBy.Name;
    public SortOrder SortOrder { get; set; } = SortOrder.Ascending;
}
