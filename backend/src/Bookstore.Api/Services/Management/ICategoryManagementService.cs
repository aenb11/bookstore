using Bookstore.Api.Common.Pagination;
using Bookstore.Api.DTOs.Categories;

namespace Bookstore.Api.Services.Management;

public interface ICategoryManagementService
{
    Task<PagedResult<CategoryDetailResponseDto>> GetCategoriesAsync(
        CategoryManagementFilterParam f,
        CancellationToken ct = default
    );
    Task<CategoryDetailResponseDto> CreateCategoryAsync(
        CategoryPostDto categoryPostDto,
        CancellationToken ct = default
    );
    Task<CategoryDetailResponseDto> UpdateCategoryAsync(
        int categoryId,
        CategoryPutDto categoryPutDto,
        CancellationToken ct = default
    );
    Task DeleteCategoryAsync(int categoryId, CancellationToken ct = default);
}
