using Bookstore.Api.Common;
using Bookstore.Api.Common.Pagination;
using Bookstore.Api.Common.Projections;
using Bookstore.Api.DTOs.Authors;
using Bookstore.Api.Extensions;
using Bookstore.Api.Queries;
using Bookstore.Data;
using Bookstore.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Api.Services.Management;

public class AuthorManagementService(AppDbContext ctx) : IAuthorManagementService
{
    public async Task<AuthorDetailResponseDto> CreateAuthorAsync(
        AuthorPostDto dto,
        CancellationToken ct = default
    )
    {
        var author = new Author
        {
            FullName = dto.FullName.Trim(),
            Nationality = dto.Nationality.Trim().ToUpperInvariant(),
            BirthYear = dto.BirthYear,
            DeathYear = dto.DeathYear,
            Biography = dto.Biography?.Trim(),
            PhotoKey = dto.PhotoKey?.Trim(),
        };

        await ctx.Authors.AddAsync(author, ct);
        await ctx.SaveChangesAsync(ct);

        return AuthorProjection.CompileToDetailResponseDto(author);
    }

    public async Task DeleteAuthorAsync(int authorId, CancellationToken ct = default)
    {
        var author =
            await ctx.Authors.FirstOrDefaultAsync(a => a.Id == authorId && !a.IsDeleted, ct)
            ?? throw new NotFoundException($"Author with ID {authorId} not found.");

        ctx.Authors.Remove(author);
        await ctx.SaveChangesAsync(ct);
    }

    public async Task<PagedResult<AuthorDetailResponseDto>> GetAllAuthorsAsync(
        AuthorManagementFilterParam f,
        CancellationToken ct = default
    )
    {
        return await ctx
            .Authors.AsNoTracking()
            .ApplyManagementFilter(f)
            .ApplyManagementSort(f)
            .Select(AuthorProjection.ToDetailResponseDto)
            .ToPagedResultAsync(f.PageNumber, f.PageSize, ct);
    }

    public async Task<AuthorDetailResponseDto?> GetAuthorByIdAsync(
        int authorId,
        CancellationToken ct = default
    )
    {
        return await ctx
            .Authors.AsNoTracking()
            .Where(a => a.Id == authorId)
            .Select(AuthorProjection.ToDetailResponseDto)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<AuthorDetailResponseDto> UpdateAuthorAsync(
        int authorId,
        AuthorPutDto dto,
        CancellationToken ct = default
    )
    {
        var author =
            await ctx.Authors.FirstOrDefaultAsync(a => a.Id == authorId, ct)
            ?? throw new NotFoundException($"Author with ID {authorId} not found.");

        author.FullName = dto.FullName.Trim();
        author.Nationality = dto.Nationality.Trim().ToUpperInvariant();
        author.BirthYear = dto.BirthYear;
        author.DeathYear = dto.DeathYear;
        author.Biography = dto.Biography?.Trim();
        author.PhotoKey = dto.PhotoKey?.Trim();

        await ctx.SaveChangesAsync(ct);

        return AuthorProjection.CompileToDetailResponseDto(author);
    }
}
