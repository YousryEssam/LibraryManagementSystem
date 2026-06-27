namespace Library.Infrastructure.Persistence.Configurations;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.ToTable("Authors");

        builder.HasKey(a => a.Id);

        // ── Columns ───────────────────────────────────────────────────────
        builder.Property(a => a.FullName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.Bio)
            .HasMaxLength(2000);

        builder.Property(a => a.Nationality)
            .HasMaxLength(100);

        // ── Indexes ───────────────────────────────────────────────────────
        builder.HasIndex(a => a.FullName)
            .HasDatabaseName("IX_Authors_FullName");

        // Soft-delete filter
        builder.HasQueryFilter(a => a.DeletedAt == null);
    }
}
