using Bookstore.Data.Common;

namespace Bookstore.Data.Entities;

public class Address : IAuditableEntity
{
    public int Id { get; init; }
    public required int AccountId { get; set; }
    public bool IsDefault { get; set; } = false;
    public required string RecipientName { get; set; }
    public required string RecipientPhone { get; set; }
    public required string Street { get; set; }
    public required string Ward { get; set; }
    public required string District { get; set; }
    public required string City { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
