using Bookstore.Api.Common.Enums;
using Bookstore.Api.Common.Extensions;
using Bookstore.Api.DTOs.Accounts;
using Bookstore.Data.Entities;

namespace Bookstore.Api.Queries;

public static class AccountQueryExtension
{
    public static IQueryable<Account> ApplyManagementFilter(
        this IQueryable<Account> q,
        AccountManagementFilterParam f
    )
    {
        var parsedRoles = f.GetParsedRoles();
        var parsedStatuses = f.GetParsedStatuses();
        return q.WhereIf(f.Email is not null, a => a.Email.Contains(f.Email!))
            .WhereIf(f.FullName is not null, a => a.FullName.Contains(f.FullName!))
            .WhereIf(f.CreatedTo.HasValue, a => a.CreatedAt <= f.CreatedTo)
            .WhereIf(f.UpdatedFrom.HasValue, a => a.UpdatedAt >= f.UpdatedFrom)
            .WhereIf(f.UpdatedTo.HasValue, a => a.UpdatedAt <= f.UpdatedTo)
            .WhereIf(parsedRoles.Count != 0, a => parsedRoles.Contains(a.Role))
            .WhereIf(parsedStatuses.Count != 0, a => parsedStatuses.Contains(a.Status))
            .WhereIf(f.CreatedFrom.HasValue, a => a.CreatedAt >= f.CreatedFrom)
            .WhereIf(f.IsDeleted.HasValue, a => a.IsDeleted == f.IsDeleted);
    }

    public static IQueryable<Account> ApplyManagementSort(
        this IQueryable<Account> query,
        AccountManagementFilterParam f
    )
    {
        var isDescending = f.SortOrder == SortOrder.Descending;

        var ordered = f.SortBy switch
        {
            AccountManagementSortBy.Email => isDescending
                ? query.OrderByDescending(a => a.Email)
                : query.OrderBy(a => a.Email),
            AccountManagementSortBy.FullName => isDescending
                ? query.OrderByDescending(a => a.FullName)
                : query.OrderBy(a => a.FullName),
            AccountManagementSortBy.Role => isDescending
                ? query.OrderByDescending(a => a.Role)
                : query.OrderBy(a => a.Role),
            AccountManagementSortBy.Status => isDescending
                ? query.OrderByDescending(a => a.Status)
                : query.OrderBy(a => a.Status),
            AccountManagementSortBy.CreatedAt => isDescending
                ? query.OrderByDescending(a => a.CreatedAt)
                : query.OrderBy(a => a.CreatedAt),
            AccountManagementSortBy.UpdatedAt => isDescending
                ? query.OrderByDescending(a => a.UpdatedAt)
                : query.OrderBy(a => a.UpdatedAt),
            _ => throw new ArgumentOutOfRangeException($"Invalid sort by value: {f.SortBy}"),
        };

        return ordered.ThenByDescending(a => a.Id);
    }
}
