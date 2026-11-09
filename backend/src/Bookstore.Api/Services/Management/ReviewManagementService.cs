using Bookstore.Api.Common;
using Bookstore.Api.Common.Pagination;
using Bookstore.Api.Common.Projections;
using Bookstore.Api.DTOs.Reviews;
using Bookstore.Api.Extensions;
using Bookstore.Api.Queries;
using Bookstore.Data;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Api.Services.Management;

public class ReviewManagementService(AppDbContext ctx) : IReviewManagementService
{
    public async Task DeleteReviewAsync(int id, CancellationToken ct = default)
    {
        var review =
            await ctx.Reviews.FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted, ct)
            ?? throw new NotFoundException($"Review with ID {id} not found.");

        ctx.Reviews.Remove(review);
        await ctx.SaveChangesAsync(ct);
    }

    public async Task<ReviewDetailResponseDto?> GetReviewByIdAsync(
        int id,
        CancellationToken ct = default
    )
    {
        return await ctx
            .Reviews.AsNoTracking()
            .Where(r => r.Id == id)
            .Select(ReviewProjection.ToDetailResponseDto)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<PagedResult<ReviewDetailResponseDto>> GetReviewsAsync(
        ReviewManagementFilterParam f,
        CancellationToken ct = default
    )
    {
        return await ctx
            .Reviews.AsNoTracking()
            .ApplyManagementFilter(f)
            .ApplySort(f)
            .Select(ReviewProjection.ToDetailResponseDto)
            .ToPagedResultAsync(f.PageNumber, f.PageSize, ct);
    }
}
