using System.Text.Json.Serialization;
using Bookstore.Api.Common.Pagination;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Bookstore.Api.DTOs.Authors;

public class AuthorFilterParam : PaginationParam
{
    public string? FullName { get; set; }
    public string? Nationalities { get; set; }

    [BindNever]
    [JsonIgnore]
    public List<string> ParsedNationalities =>
        string.IsNullOrWhiteSpace(Nationalities)
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
