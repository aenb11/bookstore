using System.Linq.Expressions;
using Bookstore.Api.DTOs.Authors;
using Bookstore.Data.Entities;

namespace Bookstore.Api.Common.Projections;

public static class AuthorProjection
{
    public static readonly Expression<Func<Author, AuthorResponseDto>> ToResponseDto =
        a => new AuthorResponseDto
        {
            Id = a.Id,
            FullName = a.FullName,
            Nationality = a.Nationality,
            BirthYear = a.BirthYear,
            DeathYear = a.DeathYear,
            Biography = a.Biography,
            PhotoKey = a.PhotoKey,
        };

    public static readonly Expression<Func<Author, AuthorDetailResponseDto>> ToDetailResponseDto =
        a => new AuthorDetailResponseDto
        {
            Id = a.Id,
            FullName = a.FullName,
            Nationality = a.Nationality,
            BirthYear = a.BirthYear,
            DeathYear = a.DeathYear,
            Biography = a.Biography,
            PhotoKey = a.PhotoKey,
            CreatedAt = a.CreatedAt,
            UpdatedAt = a.UpdatedAt,
            IsDeleted = a.IsDeleted,
            DeletedAt = a.DeletedAt,
        };

    public static readonly Func<Author, AuthorResponseDto> CompileToResponseDto =
        ToResponseDto.Compile();

    public static readonly Func<Author, AuthorDetailResponseDto> CompileToDetailResponseDto =
        ToDetailResponseDto.Compile();
}
