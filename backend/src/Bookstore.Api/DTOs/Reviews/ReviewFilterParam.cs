using Bookstore.Api.Common.Enums;
using Bookstore.Api.Common.Pagination;

namespace Bookstore.Api.DTOs.Reviews;

public enum ReviewSortBy
{
    CreatedAt,
    Rating,
}

public class ReviewFilterParam : PaginationParam
{
    public int? MinRating { get; set; }

    public int? MaxRating { get; set; }

    public DateTimeOffset? CreatedFrom { get; set; }
    public DateTimeOffset? CreatedTo { get; set; }
    public ReviewSortBy SortBy { get; set; } = ReviewSortBy.CreatedAt;
    public SortOrder SortOrder { get; set; } = SortOrder.Descending;
}
