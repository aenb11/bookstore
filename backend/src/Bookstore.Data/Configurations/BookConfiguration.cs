using Bookstore.Data.Entities;
using Bookstore.Data.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookstore.Data.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Isbn).HasMaxLength(13);
        builder
            .HasIndex(b => b.Isbn)
            .IsUnique()
            .HasFilter(SqlHelper.NotDeleted)
            .HasDatabaseName("ux_books_isbn");
        builder.Property(b => b.Title).HasMaxLength(200);
        builder.Property(b => b.Language).HasMaxLength(2); // ISO 639-1
        builder.Property(b => b.Description).HasMaxLength(1000);

        builder.Property(b => b.CoverType).HasConversion<string>().HasMaxLength(20);
        builder.Property(b => b.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(b => b.AgeGroup).HasConversion<string>().HasMaxLength(20);

        builder.ToTable(b =>
        {
            b.HasCheckConstraint("ck_books_cover_type", SqlHelper.EnumIn<BookCover>("cover_type"));
            b.HasCheckConstraint("ck_books_status", SqlHelper.EnumIn<BookStatus>("status"));
            b.HasCheckConstraint("ck_books_age_group", SqlHelper.EnumIn<BookAgeGroup>("age_group"));

            b.HasCheckConstraint("ck_books_isbn", "isbn IS NULL OR LENGTH(isbn) = 13");
            b.HasCheckConstraint(
                "ck_books_prices",
                "list_price >= 0 AND sale_price >= 0 AND sale_price <= list_price"
            );
            b.HasCheckConstraint("ck_books_stock_quantity", "stock_quantity >= 0");
            b.HasCheckConstraint("ck_books_publication_year", "publication_year >= 0");
            b.HasCheckConstraint("ck_books_weight", "weight >= 0");
            b.HasCheckConstraint("ck_books_page_count", "page_count >= 0");
            ;
        });

        builder
            .HasOne(b => b.Category)
            .WithMany()
            .HasForeignKey(b => b.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(b => b.Publisher)
            .WithMany()
            .HasForeignKey(b => b.PublisherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(b => b.Serie)
            .WithMany()
            .HasForeignKey(b => b.SerieId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(b => b.Authors).WithMany(a => a.Books).UsingEntity<BookAuthor>();
        builder.HasMany(b => b.Tags).WithMany(t => t.Books).UsingEntity<BookTag>();
    }
}
