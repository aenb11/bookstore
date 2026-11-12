using Bookstore.Api.DTOs.Addresses;

namespace Bookstore.Api.Services.Storefront;

public interface IAddressService
{
    Task<IEnumerable<AddressResponseDto>> GetAllAsync(
        int currentAccountId,
        CancellationToken ct = default
    );
    Task<AddressResponseDto> CreateAsync(
        int currentAccountId,
        AddressPostDto dto,
        CancellationToken ct = default
    );
    Task<AddressResponseDto> UpdateAsync(
        int currentAccountId,
        int addressId,
        AddressPutDto dto,
        CancellationToken ct = default
    );
    Task DeleteAsync(int currentAccountId, int addressId, CancellationToken ct = default);
}
