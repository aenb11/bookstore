using Bookstore.Api.Common.Pagination;

namespace Bookstore.Api.DTOs.Categories;

public class CategoryFilterParam : PaginationParam
{
    public string? Name { get; set; }
}
