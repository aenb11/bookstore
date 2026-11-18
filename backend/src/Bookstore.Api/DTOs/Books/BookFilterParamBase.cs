using Bookstore.Api.Common.Enums;
using Bookstore.Api.Common.Pagination;

namespace Bookstore.Api.DTOs.Books;

public abstract class BookFilterParamBase<TSortBy> : PaginationParam
    where TSortBy : struct, Enum
{
    public string? Title { get; set; }
    public string? AuthorFullName { get; set; }

    public int? CategoryId { get; set; }
    public int? SerieId { get; set; }
    public int? PublisherId { get; set; }

    public int? PublicationYearFrom { get; set; }
    public int? PublicationYearTo { get; set; }

    public int? PriceFrom { get; set; }
    public int? PriceTo { get; set; }

    public double? AverageRatingFrom { get; set; }

    public bool? IsOnStock { get; set; }
    public bool? IsDiscounted { get; set; }

    public abstract TSortBy SortBy { get; set; }
    public SortOrder SortOrder { get; set; } = SortOrder.Descending;
}
