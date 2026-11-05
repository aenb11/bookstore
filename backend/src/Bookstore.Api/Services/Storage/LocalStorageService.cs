namespace Bookstore.Api.Services.Storage;

public class LocalStorageService(IWebHostEnvironment env) : IStorageService
{
    private readonly string _root =
        Path.GetFullPath(Path.Combine(env.ContentRootPath, "uploads"))
        + Path.DirectorySeparatorChar;

    private const string PublicBaseUrl = "https://localhost:7065/files";

    public async Task<string> SaveAsync(
        Stream content,
        string fileName,
        string contentType,
        string folder,
        CancellationToken cancellationToken = default
    )
    {
        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        var key = $"{folder}/{Guid.NewGuid():N}{ext}";

        var fullPath =
            ResolveSafePath(key) ?? throw new InvalidOperationException("Invalid storage path.");

        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

        await using var fs = File.Create(fullPath);
        await content.CopyToAsync(fs, cancellationToken);
        return key;
    }

    public Task DeleteAsync(string key, CancellationToken cancellationToken = default)
    {
        var fullPath = ResolveSafePath(key);
        if (fullPath != null && File.Exists(fullPath))
            File.Delete(fullPath);
        return Task.CompletedTask;
    }

    public string GetUrl(string key) => $"{PublicBaseUrl}/{key.Replace('\\', '/')}";

    private string? ResolveSafePath(string key)
    {
        var fullPath = Path.GetFullPath(Path.Combine(_root, key));
        return fullPath.StartsWith(_root, StringComparison.Ordinal) ? fullPath : null;
    }
}
