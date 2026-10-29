namespace Bookstore.Data.Common;

public interface ISoftDeletableEntity
{
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}
