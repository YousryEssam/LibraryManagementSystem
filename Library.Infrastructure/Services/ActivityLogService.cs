namespace Library.Infrastructure.Services;

public sealed class ActivityLogService : IActivityLogService
{
    private readonly LibraryDbContext _context;

    public ActivityLogService(LibraryDbContext context) => _context = context;

    public async Task LogAsync(
        int userId,
        string action,
        string? entityName = null,
        int? entityId = null,
        string? details = null,
        string? ipAddress = null,
        CancellationToken ct = default)
    {
        _context.ActivityLogs.Add(new ActivityLog
        {
            SystemUserId = userId,
            Action = action,
            EntityName = entityName,
            EntityId = entityId,
            Details = details,
            IpAddress = ipAddress,
            Timestamp = DateTime.UtcNow
        });

        await _context.SaveChangesAsync(ct);
    }
}
