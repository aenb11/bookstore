using Bookstore.Api.Common.Enums;
using Bookstore.Api.Common.Extensions;
using Bookstore.Api.DTOs.Authors;
using Bookstore.Data.Entities;

namespace Bookstore.Api.Queries;

public static class AuthorQueryExtension
{
    public static IQueryable<Author> ApplyBaseFilter(this IQueryable<Author> q, AuthorFilterParam f)
    {
        var parsedNationalities = f.GetParsedNationalities();
        return q.WhereIf(f.FullName is not null, x => x.FullName.Contains(f.FullName!))
            .WhereIf(
                parsedNationalities.Count != 0,
                x => parsedNationalities.Contains(x.Nationality.ToUpperInvariant())
            );
    }

    public static IQueryable<Author> ApplyManagementFilter(
        this IQueryable<Author> q,
        AuthorManagementFilterParam f
    )
    {
        return q.ApplyBaseFilter(f)
            .WhereIf(f.CreatedFrom.HasValue, t => t.CreatedAt >= f.CreatedFrom)
            .WhereIf(f.CreatedTo.HasValue, t => t.CreatedAt <= f.CreatedTo)
            .WhereIf(f.UpdatedFrom.HasValue, t => t.UpdatedAt >= f.UpdatedFrom)
            .WhereIf(f.UpdatedTo.HasValue, t => t.UpdatedAt <= f.UpdatedTo)
            .WhereIf(f.IsDeleted.HasValue, t => t.IsDeleted == f.IsDeleted);
    }

    public static IQueryable<Author> ApplyManagementSort(
        this IQueryable<Author> q,
        AuthorManagementFilterParam f
    )
    {
        var isDescending = f.SortOrder == SortOrder.Descending;
        var ordered = f.SortBy switch
        {
            AuthorManagementSortBy.Name => isDescending
                ? q.OrderByDescending(t => t.FullName)
                : q.OrderBy(t => t.FullName),
            AuthorManagementSortBy.CreatedAt => isDescending
                ? q.OrderByDescending(t => t.CreatedAt)
                : q.OrderBy(t => t.CreatedAt),
            AuthorManagementSortBy.UpdatedAt => isDescending
                ? q.OrderByDescending(t => t.UpdatedAt)
                : q.OrderBy(t => t.UpdatedAt),
            _ => throw new ArgumentOutOfRangeException($"Invalid sort by value: {f.SortBy}"),
        };

        return ordered.ThenByDescending(t => t.Id);
    }
}
