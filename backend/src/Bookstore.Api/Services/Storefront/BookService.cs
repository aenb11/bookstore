using Bookstore.Api.Common.Pagination;
using Bookstore.Api.Common.Projections;
using Bookstore.Api.DTOs.Books;
using Bookstore.Api.Extensions;
using Bookstore.Api.Queries;
using Bookstore.Api.Services.Storage;
using Bookstore.Data;
using Bookstore.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Api.Services.Storefront;

internal class BookService(AppDbContext ctx, IStorageService storageService) : IBookService
{
    public async Task<PagedResult<BookSummaryResponseDto>> GetBookSummariesAsync(
        BookFilterParam f,
        CancellationToken ct = default
    )
    {
        var result = await ctx
            .Books.AsNoTracking()
            .Where(b => b.Status == BookStatus.Available && !b.IsDeleted)
            .ApplyBaseFilter(f)
            .ApplyBaseSort(f)
            .Select(BookProjection.ToSummaryResponseDto)
            .ToPagedResultAsync(f.PageNumber, f.PageSize, ct);

        foreach (var item in result.Items)
        {
            if (!string.IsNullOrEmpty(item.ThumbnailUrl))
            {
                item.ThumbnailUrl = storageService.GetUrl(item.ThumbnailUrl);
            }
        }

        return result;
    }

    public async Task<BookResponseDto?> GetBookByIdAsync(int bookId, CancellationToken ct = default)
    {
        var book = await ctx
            .Books.AsNoTracking()
            .Where(b =>
                b.Id == bookId
                && (b.Status == BookStatus.Available || b.Status == BookStatus.Discontinued)
                && !b.IsDeleted
            )
            .Select(BookProjection.ToResponseDto)
            .FirstOrDefaultAsync(ct);

        if (book is null)
            return null;

        book.Images = [.. book.Images.Select(i => i with { Url = storageService.GetUrl(i.Url) })];

        return book;
    }
}
