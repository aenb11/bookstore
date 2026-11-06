using Bookstore.Api.Common.Pagination;
using Bookstore.Api.DTOs.Reviews;

namespace Bookstore.Api.Services;

public interface IReviewService
{
    Task<ReviewResponseDto> CreateReviewAsync(
        int currentAccountId,
        string accountFullName,
        bool isCustomer,
        ReviewPostDto reviewPostDto,
        CancellationToken ct = default
    );

    Task<ReviewResponseDto> UpdateReviewAsync(
        int reviewId,
        int currentAccountId,
        string accountFullName,
        ReviewPutDto reviewPatchDto,
        CancellationToken ct = default
    );

    Task DeleteReviewAsync(
        int reviewId,
        int currentAccountId,
        bool isAdminOrStaff,
        CancellationToken ct = default
    );

    Task<ReviewDetailResponseDto?> GetReviewByIdAsync(int reviewId, CancellationToken ct = default);

    Task<PagedResult<ReviewResponseDto>> GetReviewsByBookIdAsync(
        int bookId,
        int pageNumber,
        int pageSize,
        int? excludeAccountId = null,
        CancellationToken ct = default
    );

    Task<PagedResult<ReviewResponseDto>> GetReviewsByAccountIdAsync(
        int accountId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    );

    Task<PagedResult<ReviewDetailResponseDto>> GetReviewsAsync(
        ReviewFilterParam filterParam,
        CancellationToken ct = default
    );
}
