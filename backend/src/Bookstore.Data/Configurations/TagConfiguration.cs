using Bookstore.Data.Entities;
using Bookstore.Data.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookstore.Data.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Name).HasMaxLength(100);
        builder
            .HasIndex(a => a.Name)
            .IsUnique()
            .HasFilter(SqlHelper.NotDeleted)
            .HasDatabaseName("ux_tags_name"); // Unique index with filter for soft delete
    }
}
