using System.ComponentModel.DataAnnotations;

namespace Bookstore.Api.Common.Pagination;

public class PaginationParam
{
    [Range(1, int.MaxValue, ErrorMessage = "PageNumber must be greater than or equal to 1.")]
    public int PageNumber { get; set; } = 1;

    [Range(1, 50, ErrorMessage = "PageSize must be between 1 and 50.")]
    public int PageSize { get; set; } = 10;
}
