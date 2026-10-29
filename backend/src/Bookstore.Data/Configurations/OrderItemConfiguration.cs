using Bookstore.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookstore.Data.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => new { oi.OrderId, oi.BookId });

        builder.ToTable(oi =>
        {
            oi.HasCheckConstraint("ck_order_items_quantity", "quantity > 0");
            oi.HasCheckConstraint("ck_order_items_price", "price >= 0");
        });

        builder
            .HasOne<Order>()
            .WithMany(o => o.Items)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(oi => oi.Book)
            .WithMany()
            .HasForeignKey(oi => oi.BookId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
