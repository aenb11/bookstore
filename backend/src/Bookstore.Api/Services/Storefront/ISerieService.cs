using Bookstore.Api.Common.Pagination;
using Bookstore.Api.DTOs.Series;

namespace Bookstore.Api.Services.Storefront;

public interface ISerieService
{
    Task<PagedResult<SerieResponseDto>> GetSeriesAsync(
        SerieFilterParam f,
        CancellationToken ct = default
    );
}
