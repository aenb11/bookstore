namespace Bookstore.Api.DTOs.Books;

public class BookSummaryResponseDto
{
    public int Id { get; set; }
    public string ThumbnailUrl { get; set; } = null!;
    public string Title { get; set; } = null!;
    public int ListPrice { get; set; }
    public int SalePrice { get; set; }
    public int StockQuantity { get; set; }

    public int TotalReviews { get; set; }
    public double AverageRating { get; set; }
    public int SoldCount { get; set; }
}
