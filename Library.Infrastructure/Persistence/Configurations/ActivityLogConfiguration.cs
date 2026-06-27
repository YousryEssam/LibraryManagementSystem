namespace Library.Infrastructure.Persistence.Configurations;

public class ActivityLogConfiguration : IEntityTypeConfiguration<ActivityLog>
{
    public void Configure(EntityTypeBuilder<ActivityLog> builder)
    {
        builder.ToTable("ActivityLogs");

        builder.HasKey(l => l.Id);

        // ── Columns ───────────────────────────────────────────────────────
        builder.Property(l => l.Action)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(l => l.EntityName)
            .HasMaxLength(100);

        builder.Property(l => l.Details)
            .HasMaxLength(2000);

        builder.Property(l => l.IpAddress)
            .HasMaxLength(45);

        builder.Property(l => l.Timestamp)
            .IsRequired();

        // ── Indexes ───────────────────────────────────────────────────────
        builder.HasIndex(l => l.SystemUserId)
            .HasDatabaseName("IX_ActivityLogs_SystemUserId");

        builder.HasIndex(l => l.Timestamp)
            .HasDatabaseName("IX_ActivityLogs_Timestamp");

        builder.HasIndex(l => new { l.EntityName, l.EntityId })
            .HasDatabaseName("IX_ActivityLogs_Entity");

        // ── Relationships ─────────────────────────────────────────────────
        builder.HasOne(l => l.SystemUser)
            .WithMany(u => u.ActivityLogs)
            .HasForeignKey(l => l.SystemUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
