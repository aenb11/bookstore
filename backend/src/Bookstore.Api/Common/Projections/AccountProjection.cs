using System.Linq.Expressions;
using Bookstore.Api.DTOs.Accounts;
using Bookstore.Data.Entities;

namespace Bookstore.Api.Common.Projections;

public static class AccountProjection
{
    public static readonly Expression<Func<Account, AccountDetailResponseDto>> ToDetailResponseDto =
        a => new AccountDetailResponseDto
        {
            Id = a.Id,
            Email = a.Email,
            FullName = a.FullName,
            Role = a.Role,
            Status = a.Status,
            CreatedAt = a.CreatedAt,
            UpdatedAt = a.UpdatedAt,
            IsDeleted = a.IsDeleted,
            DeletedAt = a.DeletedAt,
        };

    public static readonly Func<Account, AccountDetailResponseDto> CompileToDetailResponseDto =
        ToDetailResponseDto.Compile();
}
