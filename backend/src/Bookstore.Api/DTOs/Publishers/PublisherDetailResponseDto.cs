namespace Bookstore.Api.DTOs.Publishers;

public class PublisherDetailResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Website { get; set; }
    public string? ContactEmail { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}
