namespace Library.Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(c => c.Id);

        // ── Columns ───────────────────────────────────────────────────────
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(c => c.Description)
            .HasMaxLength(500);

        // ParentCategoryId is nullable — root categories have no parent
        builder.Property(c => c.ParentCategoryId)
            .IsRequired(false);

        // ── Indexes ───────────────────────────────────────────────────────
        builder.HasIndex(c => c.Name)
            .HasDatabaseName("IX_Categories_Name");

        builder.HasIndex(c => c.ParentCategoryId)
            .HasDatabaseName("IX_Categories_ParentCategoryId");

        // ── Self-referencing Relationship ─────────────────────────────────
        // Category → ParentCategory (many children → one parent)
        builder.HasOne(c => c.ParentCategory)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Soft-delete filter
        builder.HasQueryFilter(c => c.DeletedAt == null);
    }
}
