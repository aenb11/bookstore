using Bookstore.Data;
using Bookstore.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Npgsql;

namespace Bookstore.Api.Services;

public record CurrentAccount(
    int Id,
    string ClerkId,
    string Email,
    string FullName,
    AccountRole Role,
    AccountStatus Status,
    bool IsDeleted
);

public partial class CurrentAccountService(
    IHttpContextAccessor httpContextAccessor,
    AppDbContext db,
    IMemoryCache cache,
    ILogger<CurrentAccountService> logger
) : ICurrentAccountService
{
    public async Task<CurrentAccount> GetOrCreateCurrentAccountAsync()
    {
        var principal = httpContextAccessor.HttpContext?.User;
        var clerkId =
            principal?.FindFirst("sub")?.Value
            ?? throw new UnauthorizedAccessException("Missing sub claim.");

        var cacheKey = CacheKey(clerkId);
        if (cache.TryGetValue(cacheKey, out CurrentAccount? cached) && cached != null)
            return cached;

        var account = await FindAsync(clerkId);
        if (account == null)
        {
            var email = principal?.FindFirst("email")?.Value;
            if (string.IsNullOrWhiteSpace(email))
                throw new UnauthorizedAccessException("Session token is missing the email claim.");

            var fullName = principal?.FindFirst("full_name")?.Value;
            if (string.IsNullOrWhiteSpace(fullName))
            {
                var firstName = principal?.FindFirst("first_name")?.Value;
                var lastName = principal?.FindFirst("last_name")?.Value;
                fullName = string.Join(
                    ' ',
                    new[] { firstName, lastName }.Where(name => !string.IsNullOrWhiteSpace(name))
                );
            }
            if (string.IsNullOrWhiteSpace(fullName))
                fullName = email.Split('@')[0];
            fullName = fullName.Trim();

            await db.Accounts.AddAsync(
                new Account
                {
                    ClerkId = clerkId,
                    Email = email,
                    FullName = fullName,
                }
            );

            try
            {
                await db.SaveChangesAsync();
                LogAccountCreated(clerkId);
            }
            catch (DbUpdateException ex)
                when (ex.InnerException is PostgresException { SqlState: "23505" }) // Unique violation
            {
                db.ChangeTracker.Clear();
                LogUniqueViolation(clerkId, ex);
            }

            account =
                await FindAsync(clerkId)
                ?? throw new InvalidOperationException(
                    $"Cannot create account for {clerkId} (possibly duplicate email)"
                );
        }

        cache.Set(CacheKey(clerkId), account, TimeSpan.FromMinutes(2));
        return account;
    }

    public void Invalidate(string clerkId) => cache.Remove(CacheKey(clerkId));

    private static string CacheKey(string clerkId) => $"user_clerk_{clerkId}";

    private async Task<CurrentAccount?> FindAsync(string clerkId) =>
        await db
            .Accounts.AsNoTracking()
            .Where(a => a.ClerkId == clerkId)
            .Select(a => new CurrentAccount(
                a.Id,
                a.ClerkId,
                a.Email,
                a.FullName,
                a.Role,
                a.Status,
                a.IsDeleted
            ))
            .FirstOrDefaultAsync();

    // Log
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Created new account for Clerk ID: {ClerkId}"
    )]
    private partial void LogAccountCreated(string clerkId);

    [LoggerMessage(
        Level = LogLevel.Warning,
        Message = "Unique violation creating {ClerkId}, re-reading"
    )]
    private partial void LogUniqueViolation(string clerkId, Exception ex);
}
