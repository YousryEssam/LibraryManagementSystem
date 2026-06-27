namespace Library.Infrastructure.Persistence.Configurations;

public class BorrowingTransactionConfiguration : IEntityTypeConfiguration<BorrowingTransaction>
{
    public void Configure(EntityTypeBuilder<BorrowingTransaction> builder)
    {
        builder.ToTable("BorrowingTransactions");

        builder.HasKey(t => t.Id);

        // ── Columns ───────────────────────────────────────────────────────
        builder.Property(t => t.BorrowedAt)
            .IsRequired();

        builder.Property(t => t.DueDate)
            .IsRequired();

        builder.Property(t => t.ReturnedAt)
            .IsRequired(false);

        builder.Property(t => t.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(t => t.Notes)
            .HasMaxLength(500);

        // ── Indexes ───────────────────────────────────────────────────────
        builder.HasIndex(t => t.BookId)
            .HasDatabaseName("IX_BorrowingTransactions_BookId");

        builder.HasIndex(t => t.MemberId)
            .HasDatabaseName("IX_BorrowingTransactions_MemberId");

        builder.HasIndex(t => t.Status)
            .HasDatabaseName("IX_BorrowingTransactions_Status");

        builder.HasIndex(t => t.DueDate)
            .HasDatabaseName("IX_BorrowingTransactions_DueDate");

        // ── Relationships ─────────────────────────────────────────────────
        builder.HasOne(t => t.Book)
            .WithMany(b => b.BorrowingTransactions)
            .HasForeignKey(t => t.BookId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.Member)
            .WithMany(m => m.BorrowingTransactions)
            .HasForeignKey(t => t.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.ProcessedByUser)
            .WithMany(u => u.ProcessedTransactions)
            .HasForeignKey(t => t.ProcessedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Soft-delete filter
        builder.HasQueryFilter(t => t.DeletedAt == null);
    }
}
