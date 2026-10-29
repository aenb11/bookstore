namespace Bookstore.Data.Entities;

public class BookImage
{
    public int Id { get; init; }
    public required int BookId { get; init; }
    public required string ImageKey { get; set; }
    public required int SortOrder { get; set; }
}
