namespace Bookstore.Api.DTOs.Books;

public enum BookSortBy
{
    Title,
    Price,
    Rating,
    SoldCount,
    PublicationYear,
    CreatedAt,
}

public class BookFilterParam : BookFilterParamBase<BookSortBy>
{
    public override BookSortBy SortBy { get; set; } = BookSortBy.CreatedAt;
}
