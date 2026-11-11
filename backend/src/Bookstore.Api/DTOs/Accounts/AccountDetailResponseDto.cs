using Bookstore.Data.Entities;

namespace Bookstore.Api.DTOs.Accounts;

public class AccountDetailResponseDto
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public AccountRole Role { get; set; }
    public AccountStatus Status { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}
