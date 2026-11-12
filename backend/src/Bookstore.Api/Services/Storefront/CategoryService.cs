using Bookstore.Api.Common.Pagination;
using Bookstore.Api.Common.Projections;
using Bookstore.Api.DTOs.Categories;
using Bookstore.Api.Extensions;
using Bookstore.Api.Queries;
using Bookstore.Data;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Api.Services.Storefront;

public class CategoryService(AppDbContext ctx) : ICategoryService
{
    public Task<PagedResult<CategoryResponseDto>> GetCategoriesAsync(
        CategoryFilterParam f,
        CancellationToken ct = default
    )
    {
        return ctx
            .Categories.AsNoTracking()
            .Where(c => !c.IsDeleted)
            .ApplyBaseFilter(f)
            .OrderBy(c => c.Name)
            .Select(CategoryProjection.ToResponseDto)
            .ToPagedResultAsync(f.PageNumber, f.PageSize, ct);
    }
}
