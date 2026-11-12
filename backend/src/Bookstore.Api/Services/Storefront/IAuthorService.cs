using Bookstore.Api.Common.Pagination;
using Bookstore.Api.DTOs.Authors;

namespace Bookstore.Api.Services.Storefront;

public interface IAuthorService
{
    Task<AuthorResponseDto?> GetAuthorByIdAsync(int authorId, CancellationToken ct);
    Task<PagedResult<AuthorResponseDto>> GetAllAuthorsAsync(
        AuthorFilterParam f,
        CancellationToken ct
    );
}
