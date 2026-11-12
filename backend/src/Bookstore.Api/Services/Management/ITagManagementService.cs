using Bookstore.Api.Common.Pagination;
using Bookstore.Api.DTOs.Tags;

namespace Bookstore.Api.Services.Management;

public interface ITagManagementService
{
    Task<PagedResult<TagDetailResponseDto>> GetTagsAsync(
        TagManagementFilterParam f,
        CancellationToken ct = default
    );
    Task<TagDetailResponseDto> CreateTagAsync(
        TagPostDto tagPostDto,
        CancellationToken ct = default
    );
    Task<TagDetailResponseDto> UpdateTagAsync(
        int tagId,
        TagPutDto tagPutDto,
        CancellationToken ct = default
    );
    Task DeleteTagAsync(int tagId, CancellationToken ct = default);
}
