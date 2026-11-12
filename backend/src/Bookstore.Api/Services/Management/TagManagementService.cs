using Bookstore.Api.Common;
using Bookstore.Api.Common.Pagination;
using Bookstore.Api.Common.Projections;
using Bookstore.Api.DTOs.Tags;
using Bookstore.Api.Extensions;
using Bookstore.Api.Queries;
using Bookstore.Data;
using Bookstore.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Bookstore.Api.Services.Management;

public class TagManagementService(AppDbContext ctx) : ITagManagementService
{
    public async Task<TagDetailResponseDto> CreateTagAsync(
        TagPostDto tagPostDto,
        CancellationToken ct = default
    )
    {
        var tag = new Tag { Name = tagPostDto.Name.Trim() };

        var isNameExists = await ctx.Tags.AnyAsync(
            t => !t.IsDeleted && EF.Functions.ILike(t.Name, tag.Name),
            ct
        );

        if (isNameExists)
            throw new ConflictException($"Tag with name '{tag.Name}' already exists.");

        try
        {
            await ctx.Tags.AddAsync(tag, ct);
            await ctx.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
        {
            throw new ConflictException($"Tag with name '{tag.Name}' already exists.");
        }

        return TagProjection.CompileToDetailResponseDto(tag);
    }

    public async Task DeleteTagAsync(int tagId, CancellationToken ct = default)
    {
        var tag =
            await ctx.Tags.FirstOrDefaultAsync(t => t.Id == tagId && !t.IsDeleted, ct)
            ?? throw new NotFoundException($"Tag with ID {tagId} not found.");

        ctx.Tags.Remove(tag);
        await ctx.SaveChangesAsync(ct);
    }

    public async Task<PagedResult<TagDetailResponseDto>> GetTagsAsync(
        TagManagementFilterParam f,
        CancellationToken ct = default
    )
    {
        return await ctx
            .Tags.AsNoTracking()
            .ApplyManagementFilter(f)
            .ApplyManagementSort(f)
            .Select(TagProjection.ToDetailResponseDto)
            .ToPagedResultAsync(f.PageNumber, f.PageSize, ct);
    }

    public async Task<TagDetailResponseDto> UpdateTagAsync(
        int tagId,
        TagPutDto tagPutDto,
        CancellationToken ct = default
    )
    {
        var tag =
            await ctx.Tags.FirstOrDefaultAsync(t => t.Id == tagId, ct)
            ?? throw new NotFoundException($"Tag with ID {tagId} not found.");

        var normailizedName = tagPutDto.Name.Trim();

        if (!string.Equals(tag.Name, normailizedName, StringComparison.OrdinalIgnoreCase))
        {
            var isNameExists = await ctx.Tags.AnyAsync(
                t => !t.IsDeleted && EF.Functions.ILike(t.Name, normailizedName),
                ct
            );

            if (isNameExists)
                throw new ConflictException($"Tag with name '{normailizedName}' already exists.");

            tag.Name = normailizedName;
        }

        try
        {
            await ctx.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
        {
            throw new ConflictException($"Tag with name '{normailizedName}' already exists.");
        }

        return TagProjection.CompileToDetailResponseDto(tag);
    }
}
