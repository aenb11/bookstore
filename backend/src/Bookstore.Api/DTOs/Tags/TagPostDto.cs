using System.ComponentModel.DataAnnotations;

namespace Bookstore.Api.DTOs.Tags;

public class TagPostDto
{
    [Required(ErrorMessage = "Tag name is required.")]
    [StringLength(100, ErrorMessage = "Tag name cannot exceed 100 characters.")]
    public string Name { get; set; } = null!;
}
