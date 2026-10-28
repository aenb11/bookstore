using Bookstore.Data.Entities;
using Bookstore.Data.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookstore.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.RecipientName).HasMaxLength(100);
        builder.Property(o => o.RecipientPhone).HasMaxLength(10);

        builder.Property(o => o.Street).HasMaxLength(200);
        builder.Property(o => o.Ward).HasMaxLength(100);
        builder.Property(o => o.District).HasMaxLength(100);
        builder.Property(o => o.City).HasMaxLength(100);

        builder.Property(o => o.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(o => o.PaymentMethod).HasConversion<string>().HasMaxLength(20);
        builder.Property(o => o.PaymentStatus).HasConversion<string>().HasMaxLength(20);

        builder.ToTable(o =>
        {
            o.HasCheckConstraint("ck_orders_status", SqlHelper.EnumIn<OrderStatus>("status"));
            o.HasCheckConstraint(
                "ck_orders_payment_method",
                SqlHelper.EnumIn<PaymendMethod>("payment_method")
            );
            o.HasCheckConstraint(
                "ck_orders_payment_status",
                SqlHelper.EnumIn<PaymentStatus>("payment_status")
            );
            o.HasCheckConstraint("ck_orders_shipping_fee", "shipping_fee >= 0");
        });

        builder
            .HasOne(o => o.Account)
            .WithMany()
            .HasForeignKey(o => o.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
