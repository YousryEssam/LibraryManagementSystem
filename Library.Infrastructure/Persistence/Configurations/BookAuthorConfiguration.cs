namespace Library.Infrastructure.Persistence.Configurations;

public class BookAuthorConfiguration : IEntityTypeConfiguration<BookAuthor>
{
    public void Configure(EntityTypeBuilder<BookAuthor> builder)
    {
        builder.ToTable("BookAuthors");

        builder.HasKey(ba => ba.Id);

        builder.HasAlternateKey(ba => new { ba.BookId, ba.AuthorId });

        // ── Columns ───────────────────────────────────────────────────────
        builder.Property(ba => ba.Role)
            .IsRequired()
            .HasConversion<int>()
            .HasDefaultValue(BookAuthorRole.Author);

        // ── Indexes ───────────────────────────────────────────────────────
        builder.HasIndex(ba => ba.BookId)
            .HasDatabaseName("IX_BookAuthors_BookId");

        builder.HasIndex(ba => ba.AuthorId)
            .HasDatabaseName("IX_BookAuthors_AuthorId");

        // ── Relationships ─────────────────────────────────────────────────
        builder.HasOne(ba => ba.Book)
            .WithMany(b => b.BookAuthors)
            .HasForeignKey(ba => ba.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ba => ba.Author)
            .WithMany(a => a.BookAuthors)
            .HasForeignKey(ba => ba.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}