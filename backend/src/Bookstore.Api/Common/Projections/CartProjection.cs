using System.Linq.Expressions;
using Bookstore.Api.DTOs.Carts;
using Bookstore.Data.Entities;

namespace Bookstore.Api.Common.Projections;

public static class CartProjection
{
    public static readonly Expression<
        Func<CartItem, CartResponseDto.CartItemResponseDto>
    > ToItemResponseDto = ci => new CartResponseDto.CartItemResponseDto(
        ci.BookId,
        ci.Book.Title,
        ci.Book.Images.OrderBy(img => img.SortOrder).Select(img => img.ImageKey).FirstOrDefault(),
        ci.Book.ListPrice,
        ci.Book.SalePrice,
        ci.Book.StockQuantity,
        ci.Quantity,
        !ci.Book.IsDeleted && ci.Book.Status == BookStatus.Available && ci.Book.StockQuantity > 0
    );
}
