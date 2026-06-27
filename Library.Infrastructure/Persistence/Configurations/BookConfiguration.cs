namespace Library.Infrastructure.Persistence.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books");

        builder.HasKey(b => b.Id);

        // ── Columns ──────────────────────────────────────────────────────
        builder.Property(b => b.Title)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(b => b.ISBN)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(b => b.Edition)
            .HasMaxLength(50);

        builder.Property(b => b.Language)
            .HasMaxLength(50);

        builder.Property(b => b.Summary)
            .HasMaxLength(2000);

        builder.Property(b => b.CoverImageUrl)
            .HasMaxLength(500);

        builder.Property(b => b.Status)
            .IsRequired()
            .HasConversion<int>();

        // ── Indexes ───────────────────────────────────────────────────────
        builder.HasIndex(b => b.ISBN)
            .IsUnique()
            .HasDatabaseName("IX_Books_ISBN");

        builder.HasIndex(b => b.Title)
            .HasDatabaseName("IX_Books_Title");

        builder.HasIndex(b => b.Status)
            .HasDatabaseName("IX_Books_Status");

        builder.HasIndex(b => b.PublisherId)
            .HasDatabaseName("IX_Books_PublisherId");

        builder.HasIndex(b => b.CategoryId)
            .HasDatabaseName("IX_Books_CategoryId");

        // Soft-delete filter
        builder.HasQueryFilter(b => b.DeletedAt == null);

        // ── Relationships ─────────────────────────────────────────────────
        builder.HasOne(b => b.Publisher)
            .WithMany(p => p.Books)
            .HasForeignKey(b => b.PublisherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Category)
            .WithMany(c => c.Books)
            .HasForeignKey(b => b.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
