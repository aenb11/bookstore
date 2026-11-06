using Bookstore.Api.Common;
using Bookstore.Api.Common.Pagination;
using Bookstore.Api.DTOs.Reviews;
using Bookstore.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Api.Controllers;

[ApiController]
[Route("api/v1/reviews")]
public class ReviewController(
    IReviewService reviewService,
    ICurrentAccountService currentAccountService
) : ControllerBase
{
    [HttpGet("me")]
    [Authorize(Roles = $"{AppRole.Customer}")]
    public async Task<IActionResult> GetMyReviews(
        [FromQuery] PaginationParam paginationParam,
        CancellationToken ct
    )
    {
        var accountId = User.GetAccountId();
        var reviews = await reviewService.GetReviewsByAccountIdAsync(
            accountId,
            paginationParam.PageNumber,
            paginationParam.PageSize,
            ct
        );
        return Ok(reviews);
    }

    [HttpGet("{reviewId:int}")]
    [Authorize(Roles = $"{AppRole.Admin}, {AppRole.Staff}")]
    public async Task<IActionResult> GetReviewById(int reviewId, CancellationToken ct)
    {
        var review = await reviewService.GetReviewByIdAsync(reviewId, ct);
        if (review is null)
            return Problem(statusCode: 404, title: $"Review {reviewId} not found.");

        return Ok(review);
    }

    [HttpGet]
    [Authorize(Roles = $"{AppRole.Admin}, {AppRole.Staff}")]
    public async Task<IActionResult> GetReviews(
        [FromQuery] ReviewFilterParam filterParam,
        CancellationToken ct
    )
    {
        var reviews = await reviewService.GetReviewsAsync(filterParam, ct);
        return Ok(reviews);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateReview(
        [FromBody] ReviewPostDto dto,
        CancellationToken ct
    )
    {
        var account = await currentAccountService.GetOrCreateCurrentAccountAsync();
        var accountId = account.Id;
        var fullName = account.FullName;
        var isCustomer = User.IsInRole(AppRole.Customer);
        var review = await reviewService.CreateReviewAsync(
            accountId,
            fullName,
            isCustomer,
            dto,
            ct
        );

        return CreatedAtAction(nameof(GetReviewById), new { reviewId = review.Id }, review);
    }

    [HttpPut("{reviewId:int}")]
    [Authorize(Roles = $"{AppRole.Customer}")]
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
    [Authorize]
    public async Task<IActionResult> DeleteReview(int reviewId, CancellationToken ct)
    {
        var account = await currentAccountService.GetOrCreateCurrentAccountAsync();
        var accountId = account.Id;
        var isAdminOrStaff = User.IsInRole(AppRole.Admin) || User.IsInRole(AppRole.Staff);

        await reviewService.DeleteReviewAsync(reviewId, accountId, isAdminOrStaff, ct);

        return NoContent();
    }
}
