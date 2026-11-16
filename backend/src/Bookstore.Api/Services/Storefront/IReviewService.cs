using Bookstore.Api.Common.Pagination;
using Bookstore.Api.DTOs.Reviews;

namespace Bookstore.Api.Services.Storefront;

public interface IReviewService
{
    Task<ReviewResponseDto> CreateReviewAsync(
        int currentAccountId,
        ReviewPostDto reviewPostDto,
        CancellationToken ct = default
    );

    Task<ReviewResponseDto> UpdateReviewAsync(
        int reviewId,
        int currentAccountId,
        ReviewPutDto reviewPatchDto,
        CancellationToken ct = default
    );

    Task DeleteReviewAsync(int reviewId, int currentAccountId, CancellationToken ct = default);

    Task<PagedResult<ReviewResponseDto>> GetOwnReviewsAsync(
        int accountId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    );

    Task<PagedResult<ReviewResponseDto>> GetReviewsAsync(
        int bookId,
        ReviewFilterParam filterParam,
        CancellationToken ct = default
    );
}
