using Bookstore.Api.Common.Pagination;

namespace Bookstore.Api.DTOs.Publishers;

public class PublisherFilterParam : PaginationParam
{
    public string? Name { get; set; }
}
