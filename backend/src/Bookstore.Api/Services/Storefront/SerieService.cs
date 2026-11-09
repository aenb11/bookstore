using Bookstore.Api.Common.Pagination;
using Bookstore.Api.Common.Projections;
using Bookstore.Api.DTOs.Series;
using Bookstore.Api.Extensions;
using Bookstore.Api.Queries;
using Bookstore.Data;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Api.Services.Storefront;

public class SerieService(AppDbContext ctx) : ISerieService
{
    public async Task<PagedResult<SerieResponseDto>> GetSeriesAsync(
        SerieFilterParam f,
        CancellationToken ct = default
    )
    {
        return await ctx
            .Series.AsNoTracking()
            .Where(s => !s.IsDeleted)
            .ApplyBaseFilter(f)
            .OrderBy(s => s.Name)
            .Select(SerieProjection.ToResponseDto)
            .ToPagedResultAsync(f.PageNumber, f.PageSize, ct);
    }
}
