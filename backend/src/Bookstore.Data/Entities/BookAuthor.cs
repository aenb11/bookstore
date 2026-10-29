namespace Bookstore.Data.Entities;

public enum BookAuthorRole
{
    Author,
    Translator,
    Illustrator,
    Editor,
}

public class BookAuthor
{
    public required int BookId { get; init; }
    public required int AuthorId { get; init; }
    public required BookAuthorRole Role { get; set; }
}
