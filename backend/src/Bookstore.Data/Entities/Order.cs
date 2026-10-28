using Bookstore.Data.Common;

namespace Bookstore.Data.Entities;

public enum OrderStatus
{
    Pending,
    Confirmed,
    Shipped,
    Delivered,
    Cancelled,
}

public enum PaymendMethod
{
    CashOnDelivery,
    CreditCard,
}

public enum PaymentStatus
{
    Pending,
    Paid,
    Failed,
    Refunded,
}

public class Order : IAuditableEntity
{
    public int Id { get; init; }
    public required int AccountId { get; init; }

    public required string RecipientName { get; set; }
    public required string RecipientPhone { get; set; }
    public required string Street { get; set; }
    public required string Ward { get; set; }
    public required string District { get; set; }
    public required string City { get; set; }

    public required int ShippingFee { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public PaymendMethod PaymentMethod { get; set; } = PaymendMethod.CashOnDelivery;
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public Account Account { get; set; } = null!;
    public ICollection<OrderItem> Items { get; set; } = [];
}
