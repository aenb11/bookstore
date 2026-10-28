using Bookstore.Data.Entities;
using Bookstore.Data.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookstore.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).HasMaxLength(100);
        builder
            .HasIndex(c => c.Name)
            .IsUnique()
            .HasFilter(SqlHelper.NotDeleted)
            .HasDatabaseName("ux_categories_name"); // Unique index with filter for soft delete
    }
}
