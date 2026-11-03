namespace Bookstore.Api.Services;

public interface ICurrentAccountService
{
    Task<CurrentAccount> GetOrCreateCurrentAccountAsync();
    void Invalidate(string clerkId);
}
