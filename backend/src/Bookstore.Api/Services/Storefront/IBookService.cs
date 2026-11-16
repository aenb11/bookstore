using Bookstore.Api.Common.Pagination;
using Bookstore.Api.DTOs.Books;

namespace Bookstore.Api.Services.Storefront;

public interface IBookService
{
    Task<PagedResult<BookSummaryResponseDto>> GetBookSummariesAsync(
        BookFilterParam f,
        CancellationToken ct = default
    );

    Task<BookResponseDto?> GetBookByIdAsync(int bookId, CancellationToken ct = default);
}
