using Bookstore.Data.Entities;
using Bookstore.Data.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookstore.Data.Configurations;

public class InventoryTransactionConfiguration : IEntityTypeConfiguration<InventoryTransaction>
{
    public void Configure(EntityTypeBuilder<InventoryTransaction> builder)
    {
        builder.HasKey(it => it.Id);

        builder.Property(it => it.Reason).HasConversion<string>().HasMaxLength(20);
        builder.Property(it => it.Note).HasMaxLength(500);

        builder
            .HasIndex(it => new { it.BookId, it.CreatedAt })
            .HasDatabaseName("ix_inventory_transactions_book_id_created_at");

        builder.ToTable(it =>
        {
            it.HasCheckConstraint("ck_inventory_transactions_quantity", "quantity <> 0");
            it.HasCheckConstraint("ck_inventory_transaction_balance_after", "balance_after >= 0");
            it.HasCheckConstraint(
                "ck_inventory_transaction_reason",
                SqlHelper.EnumIn<InventoryTransactionReason>("reason")
            );
        });

        builder
            .HasOne(it => it.Book)
            .WithMany()
            .HasForeignKey(it => it.BookId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(it => it.Account)
            .WithMany()
            .HasForeignKey(it => it.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
