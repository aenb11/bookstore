using Bookstore.Api.Common.Enums;
using Bookstore.Api.Common.Pagination;
using Bookstore.Data.Entities;

namespace Bookstore.Api.DTOs.Accounts;

public enum AccountManagementSortBy
{
    Email,
    FullName,
    Role,
    Status,
    CreatedAt,
    UpdatedAt,
}

public class AccountManagementFilterParam : PaginationParam
{
    public string? Email { get; set; }
    public string? FullName { get; set; }

    public DateTimeOffset? CreatedFrom { get; set; }
    public DateTimeOffset? CreatedTo { get; set; }

    public DateTimeOffset? UpdatedFrom { get; set; }
    public DateTimeOffset? UpdatedTo { get; set; }

    public bool? IsDeleted { get; set; }

    public string? Roles { get; set; }
    public string? Statuses { get; set; }

    public AccountManagementSortBy SortBy { get; set; } = AccountManagementSortBy.CreatedAt;
    public SortOrder SortOrder { get; set; } = SortOrder.Descending;

    private List<AccountRole>? _parsedRoles;
    private List<AccountStatus>? _parsedStatuses;

    public List<AccountRole> GetParsedRoles() =>
        _parsedRoles ??= string.IsNullOrWhiteSpace(Roles)
            ? []
            :
            [
                .. Roles
                    .Split(
                        ',',
                        StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
                    )
                    .Select(r =>
                        Enum.TryParse<AccountRole>(r, ignoreCase: true, out var role)
                            ? (AccountRole?)role
                            : null
                    )
                    .Where(r => r.HasValue)
                    .Select(r => r!.Value)
                    .Distinct(),
            ];

    public List<AccountStatus> GetParsedStatuses() =>
        _parsedStatuses ??= string.IsNullOrWhiteSpace(Statuses)
            ? []
            :
            [
                .. Statuses
                    .Split(
                        ',',
                        StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
                    )
                    .Select(s =>
                        Enum.TryParse<AccountStatus>(s, ignoreCase: true, out var status)
                            ? (AccountStatus?)status
                            : null
                    )
                    .Where(s => s.HasValue)
                    .Select(s => s!.Value)
                    .Distinct(),
            ];
}
