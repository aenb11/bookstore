using System.ComponentModel.DataAnnotations;

namespace Bookstore.Api.DTOs.Series;

public class SeriePostDto
{
    [Required(ErrorMessage = "Serie name is required.")]
    [StringLength(100, ErrorMessage = "Serie name cannot exceed 100 characters.")]
    public string Name { get; set; } = null!;

    [StringLength(500, ErrorMessage = "Serie description cannot exceed 500 characters.")]
    public string? Description { get; set; }
}
