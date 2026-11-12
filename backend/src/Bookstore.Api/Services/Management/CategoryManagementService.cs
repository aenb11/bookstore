using Bookstore.Api.Common;
using Bookstore.Api.Common.Pagination;
using Bookstore.Api.Common.Projections;
using Bookstore.Api.DTOs.Categories;
using Bookstore.Api.Extensions;
using Bookstore.Api.Queries;
using Bookstore.Data;
using Bookstore.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Bookstore.Api.Services.Management;

public class CategoryManagementService(AppDbContext ctx) : ICategoryManagementService
{
    public async Task<CategoryDetailResponseDto> CreateCategoryAsync(
        CategoryPostDto categoryPostDto,
        CancellationToken ct = default
    )
    {
        var category = new Category { Name = categoryPostDto.Name.Trim() };

        var isNameExists = await ctx.Categories.AnyAsync(
            t => !t.IsDeleted && EF.Functions.ILike(t.Name, category.Name),
            ct
        );

        if (isNameExists)
            throw new ConflictException($"Category with name '{category.Name}' already exists.");

        try
        {
            await ctx.Categories.AddAsync(category, ct);
            await ctx.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
        {
            throw new ConflictException($"Category with name '{category.Name}' already exists.");
        }

        return CategoryProjection.CompileToDetailResponseDto(category);
    }

    public async Task DeleteCategoryAsync(int categoryId, CancellationToken ct = default)
    {
        var category =
            await ctx.Categories.FirstOrDefaultAsync(c => c.Id == categoryId && !c.IsDeleted, ct)
            ?? throw new NotFoundException($"Category with ID {categoryId} not found.");

        ctx.Categories.Remove(category);
        await ctx.SaveChangesAsync(ct);
    }

    public async Task<PagedResult<CategoryDetailResponseDto>> GetCategoriesAsync(
        CategoryManagementFilterParam filterParam,
        CancellationToken ct = default
    )
    {
        return await ctx
            .Categories.AsNoTracking()
            .ApplyManagementFilter(filterParam)
            .ApplyManagementSort(filterParam)
            .Select(CategoryProjection.ToDetailResponseDto)
            .ToPagedResultAsync(filterParam.PageNumber, filterParam.PageSize, ct);
    }

    public async Task<CategoryDetailResponseDto> UpdateCategoryAsync(
        int categoryId,
        CategoryPutDto categoryPutDto,
        CancellationToken ct = default
    )
    {
        var category =
            await ctx.Categories.FirstOrDefaultAsync(t => t.Id == categoryId, ct)
            ?? throw new NotFoundException($"Category with ID {categoryId} not found.");

        var normailizedName = categoryPutDto.Name.Trim();

        if (!string.Equals(category.Name, normailizedName, StringComparison.OrdinalIgnoreCase))
        {
            var isNameExists = await ctx.Categories.AnyAsync(
                t => !t.IsDeleted && EF.Functions.ILike(t.Name, normailizedName),
                ct
            );

            if (isNameExists)
                throw new ConflictException(
                    $"Category with name '{normailizedName}' already exists."
                );

            category.Name = normailizedName;
        }

        try
        {
            await ctx.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
        {
            throw new ConflictException($"Category with name '{normailizedName}' already exists.");
        }

        return CategoryProjection.CompileToDetailResponseDto(category);
    }
}
