using Bookstore.Data.Entities;
using Bookstore.Data.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookstore.Data.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Comment).HasMaxLength(1000);

        builder
            .HasIndex(r => new { r.AccountId, r.BookId })
            .IsUnique()
            .HasFilter(SqlHelper.NotDeleted)
            .HasDatabaseName("ux_reviews_account_book"); // Unique index with filter for soft delete

        builder.ToTable(b => b.HasCheckConstraint("ck_reviews_rating", "rating BETWEEN 1 AND 5"));

        builder
            .HasOne(r => r.Book)
            .WithMany()
            .HasForeignKey(r => r.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(r => r.Account)
            .WithMany()
            .HasForeignKey(r => r.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
