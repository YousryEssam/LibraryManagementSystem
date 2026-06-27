namespace Library.Infrastructure.Persistence.Configurations;

public class SystemUserConfiguration : IEntityTypeConfiguration<SystemUser>
{
    public void Configure(EntityTypeBuilder<SystemUser> builder)
    {
        builder.ToTable("SystemUsers");

        builder.HasKey(u => u.Id);

        // ── Columns ───────────────────────────────────────────────────────
        builder.Property(u => u.FullName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(u => u.Role)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(u => u.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // ── Indexes ───────────────────────────────────────────────────────
        builder.HasIndex(u => u.Username)
            .IsUnique()
            .HasDatabaseName("IX_SystemUsers_Username");

        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("IX_SystemUsers_Email");

        builder.HasIndex(u => u.Role)
            .HasDatabaseName("IX_SystemUsers_Role");

        // Soft-delete filter
        builder.HasQueryFilter(u => u.DeletedAt == null);
    }
}
