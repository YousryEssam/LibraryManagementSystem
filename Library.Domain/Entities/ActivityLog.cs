namespace Library.Domain.Entities;

public class ActivityLog : BaseEntity
{
    public int SystemUserId { get; set; }

    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// e.g. "Book", "Member", "BorrowingTransaction"
    /// </summary>
    public string? EntityName { get; set; }

    /// <summary>
    /// The ID of the affected record (if applicable).
    /// </summary>
    public int? EntityId { get; set; }

    /// <summary>
    /// JSON snapshot or description of what changed.
    /// </summary>
    public string? Details { get; set; }

    public string? IpAddress { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public SystemUser SystemUser { get; set; } = null!;
}
