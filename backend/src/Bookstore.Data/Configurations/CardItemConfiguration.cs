using Bookstore.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookstore.Data.Configurations;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.HasKey(ci => new { ci.AccountId, ci.BookId });

        builder.ToTable(ci => ci.HasCheckConstraint("ck_cart_items_quantity", "quantity > 0"));

        builder
            .HasOne(ci => ci.Book)
            .WithMany()
            .HasForeignKey(ci => ci.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne<Account>()
            .WithMany()
            .HasForeignKey(ci => ci.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
