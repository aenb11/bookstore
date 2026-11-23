using System.ComponentModel.DataAnnotations;

namespace Bookstore.Api.DTOs.Carts;

public class CartPutDto
{
    [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100.")]
    public int Quantity { get; set; } = 1;
}
