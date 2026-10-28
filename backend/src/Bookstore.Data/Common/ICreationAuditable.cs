namespace Bookstore.Data.Common;

public interface ICreationAuditable
{
    public DateTimeOffset CreatedAt { get; set; }
}
