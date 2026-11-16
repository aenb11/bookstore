using Bookstore.Api.Common.Enums;

namespace Bookstore.Api.DTOs.Categories;

public enum CategoryManagementSortBy
{
    Name,
    CreatedAt,
    UpdatedAt,
}

public class CategoryManagementFilterParam : CategoryFilterParam
{
    public DateTimeOffset? CreatedFrom { get; set; }
    public DateTimeOffset? CreatedTo { get; set; }

    public DateTimeOffset? UpdatedFrom { get; set; }
    public DateTimeOffset? UpdatedTo { get; set; }

    public bool? IsDeleted { get; set; }

    public CategoryManagementSortBy SortBy { get; set; } = CategoryManagementSortBy.Name;
    public SortOrder SortOrder { get; set; } = SortOrder.Ascending;
}
