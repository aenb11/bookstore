using Bookstore.Data.Common;

namespace Bookstore.Data.Entities;

public class CartItem : ICreationAuditable
{
    public required int AccountId { get; init; }
    public required int BookId { get; init; }
    public int Quantity { get; set; } = 1;

    public DateTimeOffset CreatedAt { get; set; }

    public Book Book { get; set; } = null!;
}
