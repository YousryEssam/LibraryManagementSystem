namespace Library.Infrastructure.Persistence.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.ToTable("Members");

        builder.HasKey(m => m.Id);

        // ── Columns ───────────────────────────────────────────────────────
        builder.Property(m => m.FullName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(m => m.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(m => m.Phone)
            .HasMaxLength(20);

        builder.Property(m => m.Address)
            .HasMaxLength(500);

        builder.Property(m => m.Status)
            .IsRequired()
            .HasConversion<int>();

        // ── Indexes ───────────────────────────────────────────────────────
        builder.HasIndex(m => m.Email)
            .IsUnique()
            .HasDatabaseName("IX_Members_Email");

        builder.HasIndex(m => m.Status)
            .HasDatabaseName("IX_Members_Status");

        builder.HasIndex(m => m.FullName)
            .HasDatabaseName("IX_Members_FullName");

        // Soft-delete filter
        builder.HasQueryFilter(m => m.DeletedAt == null);
    }
}
