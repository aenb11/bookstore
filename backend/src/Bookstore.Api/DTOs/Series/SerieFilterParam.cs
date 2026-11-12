using Bookstore.Api.Common.Pagination;

namespace Bookstore.Api.DTOs.Series;

public class SerieFilterParam : PaginationParam
{
    public string? Name { get; set; }
}
