using Bookstore.Data;
using Bookstore.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Api.Services.Common;

internal class ReviewOperationsService(AppDbContext ctx)
{
    public async Task ExecuteDeleteAndRecalculateAsync(
        Review review,
        CancellationToken ct = default
    )
    {
        var bookId = review.BookId;

        await using var transaction = await ctx.Database.BeginTransactionAsync(ct);

        ctx.Reviews.Remove(review);
        await ctx.SaveChangesAsync(ct);

        await RecalculateBookRatingAsync(bookId, ct);

        await transaction.CommitAsync(ct);
    }

    public async Task RecalculateBookRatingAsync(int bookId, CancellationToken ct = default)
    {
        await ctx
            .Books.Where(b => b.Id == bookId)
            .ExecuteUpdateAsync(
                setters =>
                    setters
                        .SetProperty(
                            b => b.TotalReviews,
                            b => ctx.Reviews.Count(r => r.BookId == bookId && !r.IsDeleted)
                        )
                        .SetProperty(
                            b => b.AverageRating,
                            b =>
                                Math.Round(
                                    ctx.Reviews.Where(r => r.BookId == bookId && !r.IsDeleted)
                                        .Select(r => (double?)r.Rating)
                                        .Average()
                                        ?? 0.0,
                                    1
                                )
                        ),
                ct
            );
    }
}
