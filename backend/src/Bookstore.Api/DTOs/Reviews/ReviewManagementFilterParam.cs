using System.ComponentModel.DataAnnotations;

namespace Bookstore.Api.DTOs.Reviews;

public class ReviewManagementFilterParam : ReviewFilterParam
{
    [Range(1, int.MaxValue, ErrorMessage = "BookId must be a positive integer.")]
    public int? BookId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "AccountId must be a positive integer.")]
    public int? AccountId { get; set; }

    public bool? IsDeleted { get; set; }
}
