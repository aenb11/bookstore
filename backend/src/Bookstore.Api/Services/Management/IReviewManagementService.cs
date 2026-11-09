using Bookstore.Api.Common.Pagination;
using Bookstore.Api.DTOs.Reviews;

namespace Bookstore.Api.Services.Management;

public interface IReviewManagementService
{
    Task DeleteReviewAsync(int id, CancellationToken ct = default);

    Task<ReviewDetailResponseDto?> GetReviewByIdAsync(int id, CancellationToken ct = default);

    Task<PagedResult<ReviewDetailResponseDto>> GetReviewsAsync(
        ReviewManagementFilterParam f,
        CancellationToken ct = default
    );
}
