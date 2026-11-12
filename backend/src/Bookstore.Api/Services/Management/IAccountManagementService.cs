using Bookstore.Api.Common.Pagination;
using Bookstore.Api.DTOs.Accounts;

namespace Bookstore.Api.Services.Management;

public interface IAccountManagementService
{
    Task<AccountDetailResponseDto?> GetByIdAsync(int id, CancellationToken ct = default);

    Task<PagedResult<AccountDetailResponseDto>> GetAllAsync(
        AccountManagementFilterParam f,
        CancellationToken ct = default
    );

    Task<AccountDetailResponseDto> UpdateAsync(
        int id,
        AccountPatchDto dto,
        CancellationToken ct = default
    );
}
