namespace Bookstore.Api.DTOs.Carts;

public class CartResponseDto
{
    public List<CartItemResponseDto> Items { get; set; } = [];

    public record CartItemResponseDto(
        int BookId,
        string Title,
        string? CoverImageUrl,
        int ListPrice,
        int SalePrice,
        int StockQuantity,
        int Quantity,
        bool IsAvailable
    );
}
