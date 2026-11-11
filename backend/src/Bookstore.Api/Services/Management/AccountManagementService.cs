using Bookstore.Api.Common;
using Bookstore.Api.Common.Pagination;
using Bookstore.Api.Common.Projections;
using Bookstore.Api.DTOs.Accounts;
using Bookstore.Api.Extensions;
using Bookstore.Api.Queries;
using Bookstore.Data;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Api.Services.Management;

public class AccountManagementService(AppDbContext ctx) : IAccountManagementService
{
    public async Task<PagedResult<AccountDetailResponseDto>> GetAllAsync(
        AccountManagementFilterParam f,
        CancellationToken ct = default
    )
    {
        return await ctx
            .Accounts.AsNoTracking()
            .ApplyManagementFilter(f)
            .ApplyManagementSort(f)
            .Select(AccountProjection.ToDetailResponseDto)
            .ToPagedResultAsync(f.PageNumber, f.PageSize, ct);
    }

    public async Task<AccountDetailResponseDto?> GetByIdAsync(
        int id,
        CancellationToken ct = default
    )
    {
        return await ctx
            .Accounts.AsNoTracking()
            .Where(a => a.Id == id)
            .Select(AccountProjection.ToDetailResponseDto)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<AccountDetailResponseDto> UpdateAsync(
        int id,
        AccountPatchDto dto,
        CancellationToken ct = default
    )
    {
        var account =
            await ctx.Accounts.FirstOrDefaultAsync(a => a.Id == id, ct)
            ?? throw new NotFoundException($"Account with ID {id} not found.");

        account.Role = dto.Role ?? account.Role;
        account.Status = dto.Status ?? account.Status;

        ctx.Accounts.Update(account);
        await ctx.SaveChangesAsync(ct);

        return AccountProjection.CompileToDetailResponseDto(account);
    }
}
