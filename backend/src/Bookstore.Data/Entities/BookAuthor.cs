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
    public required int BookId { get; set; }
    public required int AuthorId { get; set; }
    public required BookAuthorRole Role { get; set; }
}
