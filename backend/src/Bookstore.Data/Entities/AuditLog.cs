using Bookstore.Data.Common;

namespace Bookstore.Data.Entities;

public class AuditLog : ICreationAuditable
{
    public int Id { get; init; }
    public int? AccountId { get; set; }
    public required string Action { get; set; }
    public required string EntityName { get; set; }
    public int? EntityId { get; set; }
    public required string Details { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public Account? Account { get; set; }
}
