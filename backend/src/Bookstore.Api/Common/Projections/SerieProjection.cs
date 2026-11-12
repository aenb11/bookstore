using System.Linq.Expressions;
using Bookstore.Api.DTOs.Series;
using Bookstore.Data.Entities;

namespace Bookstore.Api.Common.Projections;

public static class SerieProjection
{
    public static readonly Expression<Func<Serie, SerieResponseDto>> ToResponseDto =
        s => new SerieResponseDto
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
        };

    public static readonly Expression<Func<Serie, SerieDetailResponseDto>> ToDetailResponseDto =
        s => new SerieDetailResponseDto
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            IsDeleted = s.IsDeleted,
            DeletedAt = s.DeletedAt,
        };

    public static readonly Func<Serie, SerieResponseDto> CompileToResponseDto =
        ToResponseDto.Compile();

    public static readonly Func<Serie, SerieDetailResponseDto> CompileToDetailResponseDto =
        ToDetailResponseDto.Compile();
}
