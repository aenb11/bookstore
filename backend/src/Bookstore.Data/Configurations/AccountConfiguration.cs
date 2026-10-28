using Bookstore.Data.Entities;
using Bookstore.Data.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookstore.Data.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.ClerkId).HasMaxLength(50);
        builder.HasIndex(a => a.ClerkId).IsUnique().HasDatabaseName("ux_accounts_clerk_id");

        builder.Property(a => a.Email).HasMaxLength(100);
        builder.HasIndex(a => a.Email).IsUnique().HasDatabaseName("ux_accounts_email");

        builder.Property(a => a.FullName).HasMaxLength(100);

        builder.Property(a => a.AvatarKey).HasMaxLength(255);

        builder.Property(a => a.Role).HasConversion<string>().HasMaxLength(20);

        builder.Property(a => a.Status).HasConversion<string>().HasMaxLength(20);

        builder.ToTable(a =>
        {
            a.HasCheckConstraint("ck_account_role", SqlHelper.EnumIn<AccountRole>("role"));
            a.HasCheckConstraint("ck_account_status", SqlHelper.EnumIn<AccountStatus>("status"));
        });
    }
}
