namespace Bookstore.Api.DTOs.Books;

public class BookCardResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string CoverImageUrl { get; set; } = null!;
    public int ListPrice { get; set; }
    public int SalePrice { get; set; }
}
