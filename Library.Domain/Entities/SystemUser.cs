using Microsoft.AspNetCore.Identity;

namespace Library.Domain.Entities;

public class SystemUser : IdentityUser<int>
{
    public string FullName { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.Staff;

    public bool IsActive { get; set; } = true;

    public DateTime? LastLoginAt { get; set; }

    // ── Audit fields
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    // ── Navigation Properties ─────────────────────────────────────────────────
    public ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
    public ICollection<BorrowingTransaction> ProcessedTransactions { get; set; } = new List<BorrowingTransaction>();
}
