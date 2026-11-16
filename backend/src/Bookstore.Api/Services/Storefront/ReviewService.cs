using Bookstore.Api.Common;
using Bookstore.Api.Common.Pagination;
using Bookstore.Api.Common.Projections;
using Bookstore.Api.DTOs.Reviews;
using Bookstore.Api.Extensions;
using Bookstore.Api.Queries;
using Bookstore.Api.Services.Common;
using Bookstore.Data;
using Bookstore.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Api.Services.Storefront;

internal class ReviewService(AppDbContext ctx, ReviewOperationsService reviewOperationsService)
    : IReviewService
{
    public async Task<ReviewResponseDto> CreateReviewAsync(
        int currentAccountId,
        ReviewPostDto reviewPostDto,
        CancellationToken ct = default
    )
    {
        var bookExists = await ctx.Books.AnyAsync(
            b =>
                b.Id == reviewPostDto.BookId
                && !b.IsDeleted
                && (b.Status == BookStatus.Available || b.Status == BookStatus.Discontinued),
            ct
        );
        if (!bookExists)
        {
            throw new NotFoundException($"Book with ID {reviewPostDto.BookId} was not found.");
        }

        var hasReviewed = await ctx.Reviews.AnyAsync(
            r =>
                r.BookId == reviewPostDto.BookId && r.AccountId == currentAccountId && !r.IsDeleted,
            ct
        );
        if (hasReviewed)
            throw new ConflictException("You have already reviewed this book.");

        await using var transaction = await ctx.Database.BeginTransactionAsync(ct);

        var review = new Review
        {
            BookId = reviewPostDto.BookId,
            AccountId = currentAccountId,
            Rating = reviewPostDto.Rating,
            Comment = reviewPostDto.Comment?.Trim(),
        };

        ctx.Reviews.Add(review);
        await ctx.SaveChangesAsync(ct);

        await reviewOperationsService.RecalculateBookRatingAsync(reviewPostDto.BookId, ct);

        await transaction.CommitAsync(ct);

        return ReviewProjection.CompileToResponseDto(review);
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

        await reviewOperationsService.ExecuteDeleteAndRecalculateAsync(review, ct);
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
        ReviewPutDto dto,
        CancellationToken ct = default
    )
    {
        var review =
            await ctx.Reviews.FirstOrDefaultAsync(
                r => r.Id == reviewId && r.AccountId == currentAccountId && !r.IsDeleted,
                ct
            ) ?? throw new NotFoundException($"Review with ID {reviewId} not found.");

        var isRatingChanged = review.Rating != dto.Rating;

        review.Rating = dto.Rating;
        review.Comment = dto.Comment?.Trim();

        if (isRatingChanged)
        {
            await using var transaction = await ctx.Database.BeginTransactionAsync(ct);

            await ctx.SaveChangesAsync(ct);
            await reviewOperationsService.RecalculateBookRatingAsync(review.BookId, ct);

            await transaction.CommitAsync(ct);
        }
        else
        {
            await ctx.SaveChangesAsync(ct);
        }

        return ReviewProjection.CompileToResponseDto(review);
    }
}
