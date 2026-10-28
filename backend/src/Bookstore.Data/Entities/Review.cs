using Bookstore.Data.Common;

namespace Bookstore.Data.Entities;

public class Review : IAuditableEntity, ISoftDeletableEntity
{
    public int Id { get; init; }
    public required int BookId { get; set; }
    public required int AccountId { get; set; }
    public required int Rating { get; set; } // 1 to 5
    public string? Comment { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTimeOffset? DeletedAt { get; set; }

    public Book Book { get; set; } = null!;
    public Account Account { get; set; } = null!;
}
