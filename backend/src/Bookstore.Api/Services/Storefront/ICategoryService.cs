using Bookstore.Api.Common.Pagination;
using Bookstore.Api.DTOs.Categories;

namespace Bookstore.Api.Services.Storefront;

public interface ICategoryService
{
    Task<PagedResult<CategoryResponseDto>> GetCategoriesAsync(
        CategoryFilterParam f,
        CancellationToken ct = default
    );
}
