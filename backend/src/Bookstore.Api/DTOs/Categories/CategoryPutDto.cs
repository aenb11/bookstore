using System.ComponentModel.DataAnnotations;

namespace Bookstore.Api.DTOs.Categories;

public class CategoryPutDto
{
    [Required(ErrorMessage = "Category name is required.")]
    [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
    public string Name { get; set; } = null!;
}
