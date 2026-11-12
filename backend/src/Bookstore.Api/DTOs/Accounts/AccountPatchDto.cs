using Bookstore.Data.Entities;

namespace Bookstore.Api.DTOs.Accounts;

public class AccountPatchDto
{
    public AccountRole? Role { get; set; }
    public AccountStatus? Status { get; set; }
}
