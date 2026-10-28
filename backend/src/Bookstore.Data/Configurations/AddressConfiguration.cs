using Bookstore.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookstore.Data.Configurations;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.RecipientName).HasMaxLength(100);
        builder.Property(a => a.RecipientPhone).HasMaxLength(10);

        builder.Property(a => a.Street).HasMaxLength(200);
        builder.Property(a => a.Ward).HasMaxLength(100);
        builder.Property(a => a.District).HasMaxLength(100);
        builder.Property(a => a.City).HasMaxLength(100);

        builder
            .HasIndex(a => a.AccountId)
            .IsUnique()
            .HasDatabaseName("ux_addresses_is_default")
            .HasFilter("is_default = true"); // Ensure only one default address per account

        builder
            .HasOne<Account>()
            .WithMany(a => a.Addresses)
            .HasForeignKey(a => a.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
