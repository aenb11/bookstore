using System.ComponentModel.DataAnnotations;

namespace Bookstore.Api.DTOs.Addresses;

public class AddressPutDto
{
    [Required(ErrorMessage = "RecipientName is required.")]
    [MaxLength(100, ErrorMessage = "RecipientName cannot exceed 100 characters.")]
    public string RecipientName { get; set; } = null!;

    [Required(ErrorMessage = "IsDefault is required.")]
    public bool IsDefault { get; set; }

    [Required(ErrorMessage = "RecipientPhone is required.")]
    [MaxLength(10, ErrorMessage = "RecipientPhone cannot exceed 10 characters.")]
    public string RecipientPhone { get; set; } = null!;

    [Required(ErrorMessage = "Street is required.")]
    [MaxLength(200, ErrorMessage = "Street cannot exceed 200 characters.")]
    public string Street { get; set; } = null!;

    [Required(ErrorMessage = "Ward is required.")]
    [MaxLength(100, ErrorMessage = "Ward cannot exceed 100 characters.")]
    public string Ward { get; set; } = null!;

    [Required(ErrorMessage = "District is required.")]
    [MaxLength(100, ErrorMessage = "District cannot exceed 100 characters.")]
    public string District { get; set; } = null!;

    [Required(ErrorMessage = "City is required.")]
    [MaxLength(100, ErrorMessage = "City cannot exceed 100 characters.")]
    public string City { get; set; } = null!;
}
