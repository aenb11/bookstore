using Bookstore.Api.Common.Pagination;
using Bookstore.Api.DTOs.Publishers;

namespace Bookstore.Api.Services.Management;

public interface IPublisherManagementService
{
    Task<PublisherDetailResponseDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PagedResult<PublisherDetailResponseDto>> GetAllAsync(
        PublisherManagementFilterParam f,
        CancellationToken ct = default
    );
    Task<PublisherDetailResponseDto> CreateAsync(
        PublisherPostDto dto,
        CancellationToken ct = default
    );
    Task<PublisherDetailResponseDto> UpdateAsync(
        int id,
        PublisherPutDto dto,
        CancellationToken ct = default
    );
    Task DeleteAsync(int id, CancellationToken ct = default);
}
