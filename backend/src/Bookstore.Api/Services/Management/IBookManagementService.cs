using Bookstore.Api.Common.Pagination;
using Bookstore.Api.DTOs.Books;

namespace Bookstore.Api.Services.Management;

public interface IBookManagementService
{
    Task<PagedResult<BookDetailSummaryResponseDto>> GetBooksAsync(
        BookManagementFilterParam f,
        CancellationToken ct = default
    );
    Task<BookDetailResponseDto?> GetBookByIdAsync(int bookId, CancellationToken ct = default);
    Task DeleteBookAsync(int bookId, CancellationToken ct = default);
    Task<BookDetailResponseDto> CreateBookAsync(BookPostDto dto, CancellationToken ct = default);
    Task<BookDetailResponseDto> UpdateBookAsync(
        int bookId,
        BookPutDto dto,
        CancellationToken ct = default
    );
}
