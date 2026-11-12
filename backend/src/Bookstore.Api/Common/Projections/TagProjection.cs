using System.Linq.Expressions;
using Bookstore.Api.DTOs.Tags;
using Bookstore.Data.Entities;

namespace Bookstore.Api.Common.Projections;

public static class TagProjection
{
    public static readonly Expression<Func<Tag, TagResponseDto>> ToResponseDto =
        t => new TagResponseDto { Id = t.Id, Name = t.Name };

    public static readonly Expression<Func<Tag, TagDetailResponseDto>> ToDetailResponseDto =
        t => new TagDetailResponseDto
        {
            Id = t.Id,
            Name = t.Name,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt,
            IsDeleted = t.IsDeleted,
            DeletedAt = t.DeletedAt,
        };

    public static readonly Func<Tag, TagResponseDto> CompileToResponseDto = ToResponseDto.Compile();

    public static readonly Func<Tag, TagDetailResponseDto> CompileToDetailResponseDto =
        ToDetailResponseDto.Compile();
}
