namespace Bookstore.Api.DTOs.Reviews;

public class ReviewDetailResponseDto
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public int AccountId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public string AccountFullName { get; set; } = null!;
    public string AccountEmail { get; set; } = null!;
    public string BookTitle { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}
