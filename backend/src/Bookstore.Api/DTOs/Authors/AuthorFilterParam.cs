using Bookstore.Api.Common.Pagination;

namespace Bookstore.Api.DTOs.Authors;

public class AuthorFilterParam : PaginationParam
{
    public string? FullName { get; set; }
    public string? Nationalities { get; set; }

    private List<string>? _parsedNationalities;

    public List<string> GetParsedNationalities() =>
        _parsedNationalities ??= string.IsNullOrWhiteSpace(Nationalities)
            ? []
            :
            [
                .. Nationalities
                    .Split(
                        ',',
                        StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
                    )
                    .Select(x => x.ToUpperInvariant())
                    .Distinct(),
            ];
}
