using Bookstore.Data.Entities;
using Bookstore.Data.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookstore.Data.Configurations;

public class PublisherConfiguration : IEntityTypeConfiguration<Publisher>
{
    public void Configure(EntityTypeBuilder<Publisher> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).HasMaxLength(100);
        builder.HasIndex(p => p.Name).IsUnique().HasFilter(SqlHelper.NotDeleted); // Unique index with filter for soft delete
        builder.Property(p => p.Website).HasMaxLength(200);
        builder.Property(p => p.ContactEmail).HasMaxLength(100);
    }
}
