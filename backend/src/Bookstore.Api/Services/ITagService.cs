using Bookstore.Api.Common.Pagination;
using Bookstore.Api.DTOs.Tags;

namespace Bookstore.Api.Services;

public interface ITagService
{
    Task<PagedResult<TagDetailResponseDto>> GetTagsAsync(
        TagFilterParam filterParam,
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
