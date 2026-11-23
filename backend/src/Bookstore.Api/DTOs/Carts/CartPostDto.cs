using System.ComponentModel.DataAnnotations;

namespace Bookstore.Api.DTOs.Carts;

public class CartPostDto
{
    [Range(1, int.MaxValue, ErrorMessage = "BookId must be greater than 0.")]
    public int BookId { get; set; }

    [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100.")]
    public int Quantity { get; set; } = 1;
}
