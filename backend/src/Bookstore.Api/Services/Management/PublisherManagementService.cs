using Bookstore.Api.Common;
using Bookstore.Api.Common.Pagination;
using Bookstore.Api.Common.Projections;
using Bookstore.Api.DTOs.Publishers;
using Bookstore.Api.Extensions;
using Bookstore.Api.Queries;
using Bookstore.Data;
using Bookstore.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Bookstore.Api.Services.Management;

public class PublisherManagementService(AppDbContext ctx) : IPublisherManagementService
{
    public async Task<PublisherDetailResponseDto> CreateAsync(
        PublisherPostDto dto,
        CancellationToken ct = default
    )
    {
        var publisher = new Publisher
        {
            Name = dto.Name.Trim(),
            Website = dto.Website?.Trim(),
            ContactEmail = dto.ContactEmail?.Trim(),
        };

        var isNameExists = await ctx.Publishers.AnyAsync(
            p => !p.IsDeleted && EF.Functions.ILike(p.Name, dto.Name),
            ct
        );

        if (isNameExists)
            throw new ConflictException($"Publisher with name '{dto.Name}' already exists.");

        try
        {
            await ctx.Publishers.AddAsync(publisher, ct);
            await ctx.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
        {
            throw new ConflictException($"Publisher with name '{dto.Name}' already exists.");
        }

        return PublisherProjection.CompileToDetailResponseDto(publisher);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var publisher =
            await ctx.Publishers.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted, ct)
            ?? throw new NotFoundException($"Publisher with ID {id} not found.");

        ctx.Publishers.Remove(publisher);
        await ctx.SaveChangesAsync(ct);
    }

    public Task<PagedResult<PublisherDetailResponseDto>> GetAllAsync(
        PublisherManagementFilterParam f,
        CancellationToken ct = default
    )
    {
        return ctx
            .Publishers.AsNoTracking()
            .ApplyManagementFilter(f)
            .ApplyManagementSort(f)
            .Select(PublisherProjection.ToDetailResponseDto)
            .ToPagedResultAsync(f.PageNumber, f.PageSize, ct);
    }

    public async Task<PublisherDetailResponseDto?> GetByIdAsync(
        int id,
        CancellationToken ct = default
    )
    {
        return await ctx
            .Publishers.AsNoTracking()
            .Where(p => p.Id == id)
            .Select(PublisherProjection.ToDetailResponseDto)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<PublisherDetailResponseDto> UpdateAsync(
        int id,
        PublisherPutDto dto,
        CancellationToken ct = default
    )
    {
        var publisher =
            ctx.Publishers.FirstOrDefault(p => p.Id == id)
            ?? throw new NotFoundException($"Publisher with ID {id} not found.");

        var normalizedName = dto.Name.Trim();

        if (!string.Equals(publisher.Name, normalizedName, StringComparison.OrdinalIgnoreCase))
        {
            var isNameExists = ctx.Publishers.Any(p =>
                !p.IsDeleted && EF.Functions.ILike(p.Name, normalizedName) && p.Id != id
            );

            if (isNameExists)
                throw new ConflictException($"Publisher with name '{dto.Name}' already exists.");
        }

        publisher.Name = dto.Name.Trim();
        publisher.Website = dto.Website?.Trim();
        publisher.ContactEmail = dto.ContactEmail?.Trim();

        try
        {
            await ctx.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
        {
            throw new ConflictException($"Publisher with name '{dto.Name}' already exists.");
        }

        return PublisherProjection.CompileToDetailResponseDto(publisher);
    }
}
