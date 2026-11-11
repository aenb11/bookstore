using Bookstore.Api.Common.Pagination;
using Bookstore.Api.Common.Projections;
using Bookstore.Api.DTOs.Publishers;
using Bookstore.Api.Extensions;
using Bookstore.Api.Queries;
using Bookstore.Data;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Api.Services.Storefront;

public class PublisherService(AppDbContext ctx) : IPublisherService
{
    public Task<PagedResult<PublisherResponseDto>> GetAllAsync(
        PublisherFilterParam f,
        CancellationToken ct = default
    )
    {
        return ctx
            .Publishers.AsNoTracking()
            .Where(p => !p.IsDeleted)
            .ApplyBaseFilter(f)
            .OrderBy(p => p.Name)
            .Select(PublisherProjection.ToResponseDto)
            .ToPagedResultAsync(f.PageNumber, f.PageSize, ct);
    }
}
