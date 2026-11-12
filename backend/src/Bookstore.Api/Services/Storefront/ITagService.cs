using Bookstore.Api.Common.Pagination;
using Bookstore.Api.DTOs.Tags;

namespace Bookstore.Api.Services.Storefront;

public interface ITagService
{
    Task<PagedResult<TagResponseDto>> GetTagsAsync(
        TagFilterParam f,
        CancellationToken ct = default
    );
}
