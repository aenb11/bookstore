using Bookstore.Data.Common;

namespace Bookstore.Data.Entities;

public enum AccountRole
{
    Admin,
    Staff,
    Customer,
}

public enum AccountStatus
{
    Active,
    Suspended,
}

public class Account : IAuditableEntity, ISoftDeletableEntity
{
    public int Id { get; set; }
    public required string ClerkId { get; init; }
    public required string Email { get; set; }
    public required string FullName { get; set; }
    public required string AvatarKey { get; set; }
    public AccountRole Role { get; set; } = AccountRole.Customer;
    public AccountStatus Status { get; set; } = AccountStatus.Active;

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTimeOffset? DeletedAt { get; set; }

    public ICollection<Address> Addresses { get; set; } = [];
}
