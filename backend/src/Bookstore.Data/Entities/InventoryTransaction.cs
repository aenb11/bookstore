using Bookstore.Data.Common;

namespace Bookstore.Data.Entities;

public enum InventoryTransactionReason
{
    PurchaseIn,
    Sale,
    OrderCancelled,
    Adjustment,
    Damaged,
}

public class InventoryTransaction : ICreationAuditable
{
    public int Id { get; init; }
    public required int BookId { get; init; }
    public int? OrderId { get; init; }
    public int? AccountId { get; init; }
    public required int QuantityChange { get; init; }
    public required int BalanceAfter { get; init; }
    public required InventoryTransactionReason Reason { get; init; }
    public string? Note { get; init; }

    public DateTimeOffset CreatedAt { get; set; }

    public Book Book { get; set; } = null!;
    public Account? Account { get; set; }
}
