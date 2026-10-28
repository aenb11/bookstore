using Bookstore.Data.Entities;
using Bookstore.Data.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookstore.Data.Configurations;

public class SerieConfiguration : IEntityTypeConfiguration<Serie>
{
    public void Configure(EntityTypeBuilder<Serie> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Name).HasMaxLength(100);
        builder
            .HasIndex(s => s.Name)
            .IsUnique()
            .HasFilter(SqlHelper.NotDeleted)
            .HasDatabaseName("ux_series_name"); // Unique index with filter for soft delete
        builder.Property(s => s.Description).HasMaxLength(500);
    }
}
