namespace Bookstore.Api.DTOs.Reviews;

public class ReviewResponseDto
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public string AccountFullName { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }
}
