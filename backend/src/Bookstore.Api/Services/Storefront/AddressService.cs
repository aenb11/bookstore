using Bookstore.Api.Common;
using Bookstore.Api.Common.Projections;
using Bookstore.Api.DTOs.Addresses;
using Bookstore.Data;
using Bookstore.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Api.Services.Storefront;

public class AddressService(AppDbContext ctx) : IAddressService
{
    public async Task<AddressResponseDto> CreateAsync(
        int currentAccountId,
        AddressPostDto dto,
        CancellationToken ct = default
    )
    {
        const int MaxAddressCount = 5;

        var addressCount = await ctx.Addresses.CountAsync(a => a.AccountId == currentAccountId, ct);

        if (addressCount >= MaxAddressCount)
            throw new BussinessException($"You can only have up to {MaxAddressCount} addresses.");

        if (dto.IsDefault)
        {
            var existingDefaultAddress = await ctx.Addresses.FirstOrDefaultAsync(
                a => a.AccountId == currentAccountId && a.IsDefault,
                ct
            );
            existingDefaultAddress?.IsDefault = false;
        }
        var address = new Address
        {
            AccountId = currentAccountId,
            IsDefault = dto.IsDefault,
            RecipientName = dto.RecipientName,
            RecipientPhone = dto.RecipientPhone,
            Street = dto.Street,
            Ward = dto.Ward,
            District = dto.District,
            City = dto.City,
        };

        await ctx.Addresses.AddAsync(address, ct);
        await ctx.SaveChangesAsync(ct);

        return AddressProjection.CompileToResponseDto(address);
    }

    public async Task DeleteAsync(
        int currentAccountId,
        int addressId,
        CancellationToken ct = default
    )
    {
        var deletedRows = await ctx
            .Addresses.Where(a => a.Id == addressId && a.AccountId == currentAccountId)
            .ExecuteDeleteAsync(ct);

        if (deletedRows == 0)
            throw new NotFoundException($"Address with ID {addressId} not found.");
    }

    public async Task<IEnumerable<AddressResponseDto>> GetAllAsync(
        int currentAccountId,
        CancellationToken ct = default
    )
    {
        return await ctx
            .Addresses.AsNoTracking()
            .Where(a => a.AccountId == currentAccountId)
            .OrderByDescending(a => a.IsDefault)
            .ThenByDescending(a => a.CreatedAt)
            .Select(AddressProjection.ToResponseDto)
            .ToListAsync(ct);
    }

    public async Task<AddressResponseDto> UpdateAsync(
        int currentAccountId,
        int addressId,
        AddressPutDto dto,
        CancellationToken ct = default
    )
    {
        var existingAddress =
            await ctx.Addresses.FirstOrDefaultAsync(
                a => a.Id == addressId && a.AccountId == currentAccountId,
                ct
            ) ?? throw new NotFoundException($"Address with ID {addressId} not found.");

        if (dto.IsDefault && !existingAddress.IsDefault)
        {
            await ctx
                .Addresses.Where(a =>
                    a.AccountId == currentAccountId && a.IsDefault && a.Id != addressId
                )
                .ExecuteUpdateAsync(s => s.SetProperty(a => a.IsDefault, false), ct);
        }

        existingAddress.IsDefault = dto.IsDefault;
        existingAddress.RecipientName = dto.RecipientName;
        existingAddress.RecipientPhone = dto.RecipientPhone;
        existingAddress.Street = dto.Street;
        existingAddress.Ward = dto.Ward;
        existingAddress.District = dto.District;
        existingAddress.City = dto.City;

        await ctx.SaveChangesAsync(ct);

        return AddressProjection.CompileToResponseDto(existingAddress);
    }
}
