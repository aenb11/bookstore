using Bookstore.Api.Common;
using Bookstore.Api.Common.Pagination;
using Bookstore.Api.Common.Projections;
using Bookstore.Api.DTOs.Tags;
using Bookstore.Api.Extensions;
using Bookstore.Api.Queries;
using Bookstore.Data;
using Bookstore.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Api.Services;

public class TagService(AppDbContext ctx) : ITagService
{
    public async Task<TagDetailResponseDto> CreateTagAsync(
        TagPostDto tagPostDto,
        CancellationToken ct = default
    )
    {
        var tag = new Tag { Name = tagPostDto.Name };

        await ctx.Tags.AddAsync(tag, ct);
        await ctx.SaveChangesAsync(ct);

        return new TagDetailResponseDto
        {
            Id = tag.Id,
            Name = tag.Name,
            CreatedAt = tag.CreatedAt,
            UpdatedAt = tag.UpdatedAt,
            IsDeleted = tag.IsDeleted,
            DeletedAt = tag.DeletedAt,
        };
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
        TagFilterParam filterParam,
        CancellationToken ct = default
    )
    {
        return await ctx
            .Tags.AsNoTracking()
            .ApplyFilter(filterParam)
            .ApplySort(filterParam)
            .Select(TagProjection.ToDetailResponseDto)
            .ToPagedResultAsync(filterParam.PageNumber, filterParam.PageSize, ct);
    }

    public async Task<TagDetailResponseDto> UpdateTagAsync(
        int tagId,
        TagPutDto tagPutDto,
        CancellationToken ct = default
    )
    {
        var tag =
            await ctx.Tags.FirstOrDefaultAsync(t => t.Id == tagId && !t.IsDeleted, ct)
            ?? throw new NotFoundException($"Tag with ID {tagId} not found.");

        tag.Name = tagPutDto.Name;

        await ctx.SaveChangesAsync(ct);

        return new TagDetailResponseDto
        {
            Id = tag.Id,
            Name = tag.Name,
            CreatedAt = tag.CreatedAt,
            UpdatedAt = tag.UpdatedAt,
            IsDeleted = tag.IsDeleted,
            DeletedAt = tag.DeletedAt,
        };
    }
}
