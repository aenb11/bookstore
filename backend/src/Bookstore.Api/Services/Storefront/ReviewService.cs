using Bookstore.Api.Common;
using Bookstore.Api.Common.Pagination;
using Bookstore.Api.Common.Projections;
using Bookstore.Api.DTOs.Reviews;
using Bookstore.Api.Extensions;
using Bookstore.Api.Queries;
using Bookstore.Data;
using Bookstore.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Api.Services.Storefront;

public class ReviewService(AppDbContext ctx) : IReviewService
{
    public async Task<ReviewResponseDto> CreateReviewAsync(
        int currentAccountId,
        string accountFullName,
        ReviewPostDto reviewPostDto,
        CancellationToken ct = default
    )
    {
        var hasReviewed = await ctx.Reviews.AnyAsync(
            r =>
                r.BookId == reviewPostDto.BookId && r.AccountId == currentAccountId && !r.IsDeleted,
            ct
        );
        if (hasReviewed)
            throw new ConflictException("You have already reviewed this book.");

        var review = new Review
        {
            BookId = reviewPostDto.BookId,
            AccountId = currentAccountId,
            Rating = reviewPostDto.Rating,
            Comment = reviewPostDto.Comment,
        };

        await ctx.Reviews.AddAsync(review, ct);
        await ctx.SaveChangesAsync(ct);

        return new ReviewResponseDto
        {
            Id = review.Id,
            BookId = review.BookId,
            Rating = review.Rating,
            Comment = review.Comment,
            CreatedAt = review.CreatedAt,
            AccountFullName = accountFullName,
        };
    }

    public async Task DeleteReviewAsync(
        int reviewId,
        int currentAccountId,
        CancellationToken ct = default
    )
    {
        var review =
            await ctx.Reviews.FirstOrDefaultAsync(
                r => r.Id == reviewId && r.AccountId == currentAccountId && !r.IsDeleted,
                ct
            ) ?? throw new NotFoundException($"Review with ID {reviewId} not found.");

        ctx.Reviews.Remove(review);
        await ctx.SaveChangesAsync(ct);
    }

    public async Task<PagedResult<ReviewResponseDto>> GetReviewsAsync(
        int bookId,
        ReviewFilterParam filterParam,
        CancellationToken ct = default
    )
    {
        return await ctx
            .Reviews.AsNoTracking()
            .Where(r => r.BookId == bookId && !r.IsDeleted)
            .ApplyBaseFilter(filterParam)
            .ApplySort(filterParam)
            .Select(ReviewProjection.ToResponseDto)
            .ToPagedResultAsync(filterParam.PageNumber, filterParam.PageSize, ct);
    }

    public async Task<PagedResult<ReviewResponseDto>> GetOwnReviewsAsync(
        int accountId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    )
    {
        return await ctx
            .Reviews.AsNoTracking()
            .Where(r => r.AccountId == accountId && !r.IsDeleted)
            .OrderByDescending(r => r.CreatedAt)
            .Select(ReviewProjection.ToResponseDto)
            .ToPagedResultAsync(pageNumber, pageSize, ct);
    }

    public async Task<ReviewResponseDto> UpdateReviewAsync(
        int reviewId,
        int currentAccountId,
        string accountFullName,
        ReviewPutDto dto,
        CancellationToken ct = default
    )
    {
        var review =
            await ctx.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId && !r.IsDeleted, ct)
            ?? throw new NotFoundException($"Review with ID {reviewId} not found.");

        if (review.AccountId != currentAccountId)
            throw new UnauthorizedException("You are not authorized to update this review.");

        review.Rating = dto.Rating;
        review.Comment = dto.Comment;

        await ctx.SaveChangesAsync(ct);

        return new ReviewResponseDto
        {
            Id = review.Id,
            BookId = review.BookId,
            Rating = review.Rating,
            Comment = review.Comment,
            CreatedAt = review.CreatedAt,
            AccountFullName = accountFullName,
        };
    }
}
