namespace Bookstore.Api.Services.Storage;

public interface IStorageService
{
    Task<string> SaveAsync(
        Stream content,
        string fileName,
        string contentType,
        string folder,
        CancellationToken cancellationToken = default
    );
    Task DeleteAsync(string key, CancellationToken cancellationToken = default);

    string GetUrl(string key);
}
