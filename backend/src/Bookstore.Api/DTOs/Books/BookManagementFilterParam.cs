using Bookstore.Data.Entities;

namespace Bookstore.Api.DTOs.Books;

public enum BookManagementSortBy
{
    Title,
    Price,
    Rating,
    SoldCount,
    PublicationYear,
    StockQuantity,
    CreatedAt,
    UpdatedAt,
}

public class BookManagementFilterParam : BookFilterParamBase<BookManagementSortBy>
{
    public string? Isbn { get; set; }
    public BookStatus? Status { get; set; }
    public bool? IsDeleted { get; set; }

    public override BookManagementSortBy SortBy { get; set; } = BookManagementSortBy.UpdatedAt;
}
