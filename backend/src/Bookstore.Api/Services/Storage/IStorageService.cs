namespace Bookstore.Api.Services.Storage;

public interface IStorageService
{
    Task<string> SaveAsync(
        Stream content,
        string fileName,
        string contentType,
        string folder,
        CancellationToken ct = default
    );
    Task DeleteAsync(string key, CancellationToken ct = default);

    string GetUrl(string key);
}
