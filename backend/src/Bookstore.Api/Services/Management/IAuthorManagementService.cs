using Bookstore.Api.Common.Pagination;
using Bookstore.Api.DTOs.Authors;

namespace Bookstore.Api.Services.Management;

public interface IAuthorManagementService
{
    Task<PagedResult<AuthorDetailResponseDto>> GetAllAuthorsAsync(
        AuthorManagementFilterParam f,
        CancellationToken ct = default
    );

    Task<AuthorDetailResponseDto?> GetAuthorByIdAsync(int authorId, CancellationToken ct = default);

    Task<AuthorDetailResponseDto> CreateAuthorAsync(
        AuthorPostDto dto,
        CancellationToken ct = default
    );

    Task<AuthorDetailResponseDto> UpdateAuthorAsync(
        int authorId,
        AuthorPutDto dto,
        CancellationToken ct = default
    );

    Task DeleteAuthorAsync(int authorId, CancellationToken ct = default);
}
