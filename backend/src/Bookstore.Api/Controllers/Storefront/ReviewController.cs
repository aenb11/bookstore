using Bookstore.Api.Common;
using Bookstore.Api.Common.Pagination;
using Bookstore.Api.DTOs.Reviews;
using Bookstore.Api.Services;
using Bookstore.Api.Services.Storefront;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Api.Controllers.Storefront;

[ApiController]
[Route("api/v1/reviews")]
[Authorize(Roles = $"{AppRole.Customer}")]
public class ReviewController(
    IReviewService reviewService,
    ICurrentAccountService currentAccountService
) : ControllerBase
{
    [HttpGet("me")]
    public async Task<IActionResult> GetMyReviews(
        [FromQuery] PaginationParam paginationParam,
        CancellationToken ct
    )
    {
        var accountId = User.GetAccountId();
        var reviews = await reviewService.GetOwnReviewsAsync(
            accountId,
            paginationParam.PageNumber,
            paginationParam.PageSize,
            ct
        );
        return Ok(reviews);
    }

    [HttpPost]
    public async Task<IActionResult> CreateReview(
        [FromBody] ReviewPostDto dto,
        CancellationToken ct
    )
    {
        var account = await currentAccountService.GetOrCreateCurrentAccountAsync();
        var accountId = account.Id;
        var fullName = account.FullName;
        var review = await reviewService.CreateReviewAsync(accountId, fullName, dto, ct);

        return CreatedAtAction("GetManagementReviewById", new { reviewId = review.Id }, review);
    }

    [HttpPut("{reviewId:int}")]
    public async Task<IActionResult> UpdateReview(
        int reviewId,
        [FromBody] ReviewPutDto dto,
        CancellationToken ct
    )
    {
        var account = await currentAccountService.GetOrCreateCurrentAccountAsync();
        var accountId = account.Id;
        var fullName = account.FullName;
        var updatedReview = await reviewService.UpdateReviewAsync(
            reviewId,
            accountId,
            fullName,
            dto,
            ct
        );

        return Ok(updatedReview);
    }

    [HttpDelete("{reviewId:int}")]
    public async Task<IActionResult> DeleteReview(int reviewId, CancellationToken ct)
    {
        var accountId = User.GetAccountId();

        await reviewService.DeleteReviewAsync(reviewId, accountId, ct);

        return NoContent();
    }
}
