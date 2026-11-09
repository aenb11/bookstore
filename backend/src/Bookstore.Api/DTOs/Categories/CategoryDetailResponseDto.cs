namespace Bookstore.Api.DTOs.Categories;

public class CategoryDetailResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}
