namespace Library.Infrastructure.Persistence.Configurations;

public class PublisherConfiguration : IEntityTypeConfiguration<Publisher>
{
    public void Configure(EntityTypeBuilder<Publisher> builder)
    {
        builder.ToTable("Publishers");

        builder.HasKey(p => p.Id);

        // ── Columns ───────────────────────────────────────────────────────
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Address)
            .HasMaxLength(500);

        builder.Property(p => p.ContactEmail)
            .HasMaxLength(150);

        builder.Property(p => p.Phone)
            .HasMaxLength(20);

        builder.Property(p => p.Website)
            .HasMaxLength(300);

        // ── Indexes ───────────────────────────────────────────────────────
        builder.HasIndex(p => p.Name)
            .HasDatabaseName("IX_Publishers_Name");

        // Soft-delete filter
        builder.HasQueryFilter(p => p.DeletedAt == null);
    }
}
