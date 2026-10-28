using Bookstore.Data.Common;

namespace Bookstore.Data.Entities;

public class Tag : IAuditableEntity, ISoftDeletableEntity
{
    public int Id { get; init; }
    public required string Name { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTimeOffset? DeletedAt { get; set; }

    public ICollection<Book> Books { get; set; } = [];
}
