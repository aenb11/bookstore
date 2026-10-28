using Bookstore.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookstore.Data.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.HasKey(al => al.Id);

        builder.Property(al => al.Action).HasMaxLength(50);

        builder.Property(al => al.EntityName).HasMaxLength(100);

        builder.Property(al => al.Details).HasColumnType("jsonb");

        builder
            .HasIndex(al => new { al.EntityName, al.EntityId })
            .HasDatabaseName("ix_audit_log_entity_name_entity_id");
        builder.HasIndex(al => al.CreatedAt).HasDatabaseName("ix_audit_log_created_at");
        builder.HasIndex(al => al.AccountId).HasDatabaseName("ix_audit_log_account_id");

        builder
            .HasOne(al => al.Account)
            .WithMany()
            .HasForeignKey(al => al.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
