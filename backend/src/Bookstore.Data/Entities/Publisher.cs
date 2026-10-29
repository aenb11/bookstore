using Bookstore.Data.Common;

namespace Bookstore.Data.Entities;

public class Publisher : IAuditableEntity, ISoftDeletableEntity
{
    public int Id { get; init; }
    public required string Name { get; set; }
    public string? Website { get; set; }
    public string? ContactEmail { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTimeOffset? DeletedAt { get; set; }
}
