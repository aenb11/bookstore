using Bookstore.Api.Common.Pagination;
using Bookstore.Api.DTOs.Publishers;

namespace Bookstore.Api.Services.Storefront;

public interface IPublisherService
{
    Task<PagedResult<PublisherResponseDto>> GetAllAsync(
        PublisherFilterParam f,
        CancellationToken ct = default
    );
}
