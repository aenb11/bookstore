using Bookstore.Data.Entities;
using Bookstore.Data.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookstore.Data.Configurations;

public class BookAuthorConfiguration : IEntityTypeConfiguration<BookAuthor>
{
    public void Configure(EntityTypeBuilder<BookAuthor> builder)
    {
        builder.HasKey(ba => new
        {
            ba.BookId,
            ba.AuthorId,
            ba.Role,
        });

        builder.Property(ba => ba.Role).HasConversion<string>().HasMaxLength(20);

        builder.ToTable(t =>
            t.HasCheckConstraint("ck_book_authors_role", SqlHelper.EnumIn<BookAuthorRole>("role"))
        );
    }
}
