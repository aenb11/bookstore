using Bookstore.Api.Common;

namespace Bookstore.Api.Services.Storage;

public enum UploadPurpose
{
    AuthorPhoto,
    BookCover,
}

public record UploadPolicies(
    string Folder,
    long MaxFileBytes,
    string[] AllowedExtensions,
    string[] AllowedRoles
);

public static class UploadPolicy
{
    private static readonly string[] Images = [".jpg", ".png", ".webp"];

    public static readonly IReadOnlyDictionary<UploadPurpose, UploadPolicies> All = new Dictionary<
        UploadPurpose,
        UploadPolicies
    >
    {
        [UploadPurpose.AuthorPhoto] = new("authors", 2 * 1024 * 1024, Images, []),
        [UploadPurpose.BookCover] = new(
            "books",
            2 * 1024 * 1024,
            Images,
            [AppRole.Admin, AppRole.Staff]
        ),
    };
}
