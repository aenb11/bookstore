using Bookstore.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookstore.Data.Configurations;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.FullName).HasMaxLength(100);
        builder.Property(a => a.Nationality).HasMaxLength(2); // ISO 3166-1 alpha-2 country code
        builder.Property(a => a.Biography).HasMaxLength(2000);
        builder.Property(a => a.PhotoKey).HasMaxLength(255);
    }
}
