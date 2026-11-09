using Bookstore.Api.Common;
using Bookstore.Api.DTOs.Reviews;
using Bookstore.Api.Services.Management;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Api.Controllers.Management;

[ApiController]
[Route("api/v1/management/reviews")]
[Authorize(Roles = $"{AppRole.Admin}, {AppRole.Staff}")]
[Tags("Management - Reviews")]
public class ReviewController(IReviewManagementService reviewManagementService) : ControllerBase
{
    [HttpGet("{reviewId:int}", Name = "GetManagementReviewById")]
    public async Task<IActionResult> GetReviewById(int reviewId, CancellationToken ct)
    {
        var review = await reviewManagementService.GetReviewByIdAsync(reviewId, ct);
        if (review is null)
            return Problem(statusCode: 404, title: $"Review {reviewId} not found.");

        return Ok(review);
    }

    [HttpGet]
    public async Task<IActionResult> GetReviews(
        [FromQuery] ReviewManagementFilterParam f,
        CancellationToken ct
    )
    {
        var reviews = await reviewManagementService.GetReviewsAsync(f, ct);
        return Ok(reviews);
    }

    [HttpDelete("{reviewId:int}")]
    [Authorize]
    public async Task<IActionResult> DeleteReview(int reviewId, CancellationToken ct)
    {
        await reviewManagementService.DeleteReviewAsync(reviewId, ct);

        return NoContent();
    }
}
