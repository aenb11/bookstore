using Bookstore.Api.Common.Pagination;
using Bookstore.Api.Common.Projections;
using Bookstore.Api.DTOs.Authors;
using Bookstore.Api.Extensions;
using Bookstore.Api.Queries;
using Bookstore.Data;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Api.Services.Storefront;

public class AuthorService(AppDbContext ctx) : IAuthorService
{
    public async Task<PagedResult<AuthorResponseDto>> GetAllAuthorsAsync(
        AuthorFilterParam f,
        CancellationToken ct
    )
    {
        return await ctx
            .Authors.AsNoTracking()
            .Where(a => !a.IsDeleted)
            .ApplyBaseFilter(f)
            .OrderBy(a => a.FullName)
            .Select(AuthorProjection.ToResponseDto)
            .ToPagedResultAsync(f.PageNumber, f.PageSize, ct);
    }

    public async Task<AuthorResponseDto?> GetAuthorByIdAsync(int authorId, CancellationToken ct)
    {
        return await ctx
            .Authors.AsNoTracking()
            .Where(a => !a.IsDeleted && a.Id == authorId)
            .Select(AuthorProjection.ToResponseDto)
            .FirstOrDefaultAsync(ct);
    }
}
