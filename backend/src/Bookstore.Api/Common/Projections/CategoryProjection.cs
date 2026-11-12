using System.Linq.Expressions;
using Bookstore.Api.DTOs.Categories;
using Bookstore.Data.Entities;

namespace Bookstore.Api.Common.Projections;

public static class CategoryProjection
{
    public static readonly Expression<Func<Category, CategoryResponseDto>> ToResponseDto =
        c => new CategoryResponseDto { Id = c.Id, Name = c.Name };

    public static readonly Expression<
        Func<Category, CategoryDetailResponseDto>
    > ToDetailResponseDto = c => new CategoryDetailResponseDto
    {
        Id = c.Id,
        Name = c.Name,
        CreatedAt = c.CreatedAt,
        UpdatedAt = c.UpdatedAt,
        IsDeleted = c.IsDeleted,
        DeletedAt = c.DeletedAt,
    };

    public static readonly Func<Category, CategoryResponseDto> CompileToResponseDto =
        ToResponseDto.Compile();

    public static readonly Func<Category, CategoryDetailResponseDto> CompileToDetailResponseDto =
        ToDetailResponseDto.Compile();
}
