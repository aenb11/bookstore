namespace Bookstore.Api.DTOs.Series;

public class SerieResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
