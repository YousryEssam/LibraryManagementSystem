namespace Library.Domain.Entities;

public class SystemUser : AuditableEntity
{
    public string FullName { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Stores BCrypt hashed password — never plain text.
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.Staff;

    public bool IsActive { get; set; } = true;

    public DateTime? LastLoginAt { get; set; }

    // Navigation Properties
    public ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
    public ICollection<BorrowingTransaction> ProcessedTransactions { get; set; } = new List<BorrowingTransaction>();
}
