namespace Bookstore.Api.DTOs.Reviews;

public class ReviewManagementFilterParam : ReviewFilterParam
{
    public int? BookId { get; set; }
    public int? AccountId { get; set; }
    public bool? IsDeleted { get; set; }
}
