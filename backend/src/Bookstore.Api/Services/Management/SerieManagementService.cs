using Bookstore.Api.Common;
using Bookstore.Api.Common.Pagination;
using Bookstore.Api.Common.Projections;
using Bookstore.Api.DTOs.Series;
using Bookstore.Api.Extensions;
using Bookstore.Api.Queries;
using Bookstore.Data;
using Bookstore.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Bookstore.Api.Services.Management;

public class SerieManagementService(AppDbContext ctx) : ISerieManagementService
{
    public async Task<SerieDetailResponseDto> CreateSerieAsync(
        SeriePostDto seriePostDto,
        CancellationToken ct = default
    )
    {
        var serie = new Serie
        {
            Name = seriePostDto.Name.Trim(),
            Description = seriePostDto.Description?.Trim(),
        };

        var isNameExists = await ctx.Series.AnyAsync(
            s => !s.IsDeleted && EF.Functions.ILike(s.Name, serie.Name),
            ct
        );

        if (isNameExists)
            throw new ConflictException($"Serie with name '{serie.Name}' already exists.");

        try
        {
            await ctx.Series.AddAsync(serie, ct);
            await ctx.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
        {
            throw new ConflictException($"Serie with name '{serie.Name}' already exists.");
        }

        return SerieProjection.CompileToDetailResponseDto(serie);
    }

    public async Task DeleteSerieAsync(int serieId, CancellationToken ct = default)
    {
        var serie =
            await ctx.Series.FirstOrDefaultAsync(t => t.Id == serieId && !t.IsDeleted, ct)
            ?? throw new NotFoundException($"Serie with ID {serieId} not found.");

        ctx.Series.Remove(serie);
        await ctx.SaveChangesAsync(ct);
    }

    public async Task<PagedResult<SerieDetailResponseDto>> GetSeriesAsync(
        SerieManagementFilterParam f,
        CancellationToken ct = default
    )
    {
        return await ctx
            .Series.AsNoTracking()
            .ApplyManagementFilter(f)
            .ApplyManagementSort(f)
            .Select(SerieProjection.ToDetailResponseDto)
            .ToPagedResultAsync(f.PageNumber, f.PageSize, ct);
    }

    public async Task<SerieDetailResponseDto> UpdateSerieAsync(
        int serieId,
        SeriePutDto seriePutDto,
        CancellationToken ct = default
    )
    {
        var serie =
            await ctx.Series.FirstOrDefaultAsync(t => t.Id == serieId, ct)
            ?? throw new NotFoundException($"Serie with ID {serieId} not found.");

        var normailizedName = seriePutDto.Name.Trim();

        if (!string.Equals(serie.Name, normailizedName, StringComparison.OrdinalIgnoreCase))
        {
            var isNameExists = await ctx.Series.AnyAsync(
                t => !t.IsDeleted && EF.Functions.ILike(t.Name, normailizedName),
                ct
            );

            if (isNameExists)
                throw new ConflictException($"Serie with name '{normailizedName}' already exists.");

            serie.Name = normailizedName;
        }

        try
        {
            await ctx.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
        {
            throw new ConflictException($"Serie with name '{normailizedName}' already exists.");
        }

        return SerieProjection.CompileToDetailResponseDto(serie);
    }
}
