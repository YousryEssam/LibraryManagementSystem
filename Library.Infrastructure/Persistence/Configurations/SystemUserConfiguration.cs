namespace Library.Infrastructure.Persistence.Configurations;


public class SystemUserConfiguration : IEntityTypeConfiguration<SystemUser>
{
    public void Configure(EntityTypeBuilder<SystemUser> builder)
    {
        builder.Property(u => u.FullName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(u => u.Role)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(u => u.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        builder.Property(u => u.ModifiedAt)
            .IsRequired(false);

        builder.Property(u => u.DeletedAt)
            .IsRequired(false);

        // ── Indexes ───────────────────────────────────────────────────────────
        // Identity already creates unique indexes on UserName and Email
        // We only add our custom ones:
        builder.HasIndex(u => u.Role)
            .HasDatabaseName("IX_SystemUsers_Role");

        builder.HasIndex(u => u.IsActive)
            .HasDatabaseName("IX_SystemUsers_IsActive");

        // Soft-delete filter
        builder.HasQueryFilter(u => u.DeletedAt == null);

        // ── Relationships ─────────────────────────────────────────────────────
        builder.HasMany(u => u.ActivityLogs)
            .WithOne(l => l.SystemUser)
            .HasForeignKey(l => l.SystemUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.ProcessedTransactions)
            .WithOne(t => t.ProcessedByUser)
            .HasForeignKey(t => t.ProcessedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}