namespace Bookstore.Data.Common;

public interface IModificationAuditable
{
    public DateTimeOffset? UpdatedAt { get; set; }
}
