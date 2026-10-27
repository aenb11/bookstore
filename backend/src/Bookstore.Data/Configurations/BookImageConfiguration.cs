using Bookstore.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookstore.Data.Configurations;

public class BookImageConfiguration : IEntityTypeConfiguration<BookImage>
{
    public void Configure(EntityTypeBuilder<BookImage> builder)
    {
        builder.HasKey(bi => bi.Id);

        builder.Property(bi => bi.ImageKey).HasMaxLength(255);

        builder.ToTable(bi =>
            bi.HasCheckConstraint("ck_book_images_sort_order", "sort_order >= 0")
        );

        builder
            .HasOne<Book>()
            .WithMany(b => b.Images)
            .HasForeignKey(bi => bi.BookId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
