namespace Bookstore.Api.DTOs.Authors;

public class AuthorDetailResponseDto
{
    public int Id { get; init; }
    public string FullName { get; set; } = null!;
    public string Nationality { get; set; } = null!;
    public int? BirthYear { get; set; }
    public int? DeathYear { get; set; }
    public string? Biography { get; set; }
    public string? PhotoKey { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}
