namespace Bookstore.Data.Entities;

public class OrderItem
{
    public int OrderId { get; init; }
    public int BookId { get; init; }
    public required int Quantity { get; set; }
    public required int Price { get; set; }

    public Book Book { get; set; } = null!;
}
