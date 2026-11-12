using Bookstore.Api.Common.Pagination;
using Bookstore.Api.DTOs.Series;

namespace Bookstore.Api.Services.Management;

public interface ISerieManagementService
{
    Task<PagedResult<SerieDetailResponseDto>> GetSeriesAsync(
        SerieManagementFilterParam f,
        CancellationToken ct = default
    );
    Task<SerieDetailResponseDto> CreateSerieAsync(
        SeriePostDto seriePostDto,
        CancellationToken ct = default
    );
    Task<SerieDetailResponseDto> UpdateSerieAsync(
        int serieId,
        SeriePutDto seriePutDto,
        CancellationToken ct = default
    );
    Task DeleteSerieAsync(int serieId, CancellationToken ct = default);
}
