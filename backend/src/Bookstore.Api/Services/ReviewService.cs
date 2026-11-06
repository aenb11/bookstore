using Bookstore.Api.Common;
using Bookstore.Api.Common.Pagination;
using Bookstore.Api.Common.Projections;
using Bookstore.Api.DTOs.Reviews;
using Bookstore.Api.Extensions;
using Bookstore.Api.Queries;
using Bookstore.Data;
using Bookstore.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Api.Services;

public class ReviewService(AppDbContext ctx) : IReviewService
{
    public async Task<ReviewResponseDto> CreateReviewAsync(
        int currentAccountId,
        string accountFullName,
        bool isCustomer,
        ReviewPostDto reviewPostDto,
        CancellationToken ct = default
    )
    {
        if (!isCustomer)
            throw new ForbiddenException("Only customers can create reviews.");

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
        bool isAdminOrStaff,
        CancellationToken ct = default
    )
    {
        var review =
            await ctx.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId && !r.IsDeleted, ct)
            ?? throw new NotFoundException($"Review with ID {reviewId} not found.");

        if (review.AccountId != currentAccountId && !isAdminOrStaff)
            throw new ForbiddenException("You are not authorized to delete this review.");

        ctx.Reviews.Remove(review);
        await ctx.SaveChangesAsync(ct);
    }

    public async Task<ReviewDetailResponseDto?> GetReviewByIdAsync(
        int reviewId,
        CancellationToken ct = default
    )
    {
        return await ctx
            .Reviews.AsNoTracking()
            .Where(r => r.Id == reviewId)
            .Select(ReviewProjections.ToDetailResponseDto)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<PagedResult<ReviewDetailResponseDto>> GetReviewsAsync(
        ReviewFilterParam filterParam,
        CancellationToken ct = default
    )
    {
        return await ctx
            .Reviews.AsNoTracking()
            .ApplyFilter(filterParam)
            .ApplySort(filterParam)
            .Select(ReviewProjections.ToDetailResponseDto)
            .ToPagedResultAsync(filterParam.PageNumber, filterParam.PageSize, ct);
    }

    public async Task<PagedResult<ReviewResponseDto>> GetReviewsByAccountIdAsync(
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
            .Select(ReviewProjections.ToResponseDto)
            .ToPagedResultAsync(pageNumber, pageSize, ct);
    }

    public async Task<PagedResult<ReviewResponseDto>> GetReviewsByBookIdAsync(
        int bookId,
        int pageNumber,
        int pageSize,
        int? excludeAccountId = null,
        CancellationToken ct = default
    )
    {
        return await ctx
            .Reviews.AsNoTracking()
            .Where(r =>
                r.BookId == bookId
                && !r.IsDeleted
                && (excludeAccountId == null || r.AccountId != excludeAccountId)
            )
            .OrderByDescending(r => r.CreatedAt)
            .Select(ReviewProjections.ToResponseDto)
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
            ?? throw new KeyNotFoundException($"Review with ID {reviewId} not found.");

        if (review.AccountId != currentAccountId)
            throw new UnauthorizedAccessException("You are not authorized to update this review.");

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
