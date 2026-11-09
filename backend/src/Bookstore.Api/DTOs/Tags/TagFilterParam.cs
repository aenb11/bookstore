using Bookstore.Api.Common.Pagination;

namespace Bookstore.Api.DTOs.Tags;

public class TagFilterParam : PaginationParam
{
    public string? Name { get; set; }
}
