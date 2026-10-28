using Bookstore.Data.Common;

namespace Bookstore.Data.Entities;

public class Author : IAuditableEntity, ISoftDeletableEntity
{
    public int Id { get; init; }
    public required string FullName { get; set; }
    public required string Nationality { get; set; }
    public int? BirthYear { get; set; }
    public int? DeathYear { get; set; }
    public string? Biography { get; set; }
    public string? PhotoKey { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTimeOffset? DeletedAt { get; set; }

    public ICollection<Book> Books { get; set; } = [];
}
