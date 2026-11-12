using System.ComponentModel.DataAnnotations;

namespace Bookstore.Api.DTOs.Authors;

public class AuthorPutDto : IValidatableObject
{
    [Required(ErrorMessage = "FullName is required.")]
    [StringLength(100, ErrorMessage = "FullName cannot exceed 100 characters.")]
    public string FullName { get; set; } = null!;

    [Required(ErrorMessage = "Nationality is required.")]
    [StringLength(2, ErrorMessage = "Nationality must be a 2-letter country code.")]
    public string Nationality { get; set; } = null!;

    [Range(1000, 9999, ErrorMessage = "BirthYear must be a 4-digit year.")]
    public int? BirthYear { get; set; }

    [Range(1000, 9999, ErrorMessage = "DeathYear must be a 4-digit year.")]
    public int? DeathYear { get; set; }

    [StringLength(2000, ErrorMessage = "Biography cannot exceed 2000 characters.")]
    public string? Biography { get; set; }

    [StringLength(255, ErrorMessage = "PhotoKey cannot exceed 255 characters.")]
    public string? PhotoKey { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (BirthYear.HasValue && DeathYear.HasValue && BirthYear > DeathYear)
        {
            yield return new ValidationResult(
                "BirthYear cannot be later than DeathYear.",
                [nameof(BirthYear), nameof(DeathYear)]
            );
        }
    }
}
