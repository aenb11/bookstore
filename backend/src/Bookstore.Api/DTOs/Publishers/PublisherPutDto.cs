using System.ComponentModel.DataAnnotations;

namespace Bookstore.Api.DTOs.Publishers;

public class PublisherPutDto
{
    [Required(ErrorMessage = "Publisher name is required.")]
    [MaxLength(100, ErrorMessage = "Publisher name cannot exceed 100 characters.")]
    public string Name { get; set; } = null!;

    [MaxLength(200, ErrorMessage = "Website URL cannot exceed 200 characters.")]
    [Url(ErrorMessage = "Website must be a valid URL.")]
    public string? Website { get; set; }

    [MaxLength(100, ErrorMessage = "Contact email cannot exceed 100 characters.")]
    [EmailAddress(ErrorMessage = "Contact email must be a valid email address.")]
    public string? ContactEmail { get; set; }
}
