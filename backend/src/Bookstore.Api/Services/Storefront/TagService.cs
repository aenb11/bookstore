using Bookstore.Api.Common.Pagination;
using Bookstore.Api.Common.Projections;
using Bookstore.Api.DTOs.Tags;
using Bookstore.Api.Extensions;
using Bookstore.Api.Queries;
using Bookstore.Data;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Api.Services.Storefront;

public class TagService(AppDbContext ctx) : ITagService
{
    public async Task<PagedResult<TagResponseDto>> GetTagsAsync(
        TagFilterParam f,
        CancellationToken ct = default
    )
    {
        return await ctx
            .Tags.AsNoTracking()
            .Where(t => !t.IsDeleted)
            .ApplyBaseFilter(f)
            .OrderBy(t => t.Name)
            .Select(TagProjection.ToResponseDto)
            .ToPagedResultAsync(f.PageNumber, f.PageSize, ct);
    }
}
