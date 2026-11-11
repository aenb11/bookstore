namespace Bookstore.Api.DTOs.Publishers;

public class PublisherResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Website { get; set; }
    public string? ContactEmail { get; set; }
}
