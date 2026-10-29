using Bookstore.Data.Common;

namespace Bookstore.Data.Entities;

public class AuditLog : ICreationAuditable
{
    public int Id { get; init; }
    public int? AccountId { get; init; }
    public required string Action { get; init; }
    public required string EntityName { get; init; }
    public int? EntityId { get; init; }
    public required string Details { get; init; }

    public DateTimeOffset CreatedAt { get; set; }

    public Account? Account { get; set; }
}
