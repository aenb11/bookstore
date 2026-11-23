using Bookstore.Api.DTOs.Carts;

namespace Bookstore.Api.Services.Storefront;

public interface ICartService
{
    Task<CartResponseDto> GetCartItemsAsync(int accountId, CancellationToken ct = default);
    Task AddCartItemAsync(int accountId, CartPostDto dto, CancellationToken ct = default);
    Task UpdateCartItemAsync(
        int accountId,
        int bookId,
        CartPutDto dto,
        CancellationToken ct = default
    );
    Task RemoveCartItemAsync(int accountId, int bookId, CancellationToken ct = default);
    Task ClearCartAsync(int accountId, CancellationToken ct = default);
}
