namespace Library.Domain.Entities;

public class Member : AuditableEntity
{
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public DateTime MembershipDate { get; set; } = DateTime.UtcNow;

    public DateTime? MembershipExpiryDate { get; set; }

    public MemberStatus Status { get; set; } = MemberStatus.Active;

    // Navigation Properties
    public ICollection<BorrowingTransaction> BorrowingTransactions { get; set; } = new List<BorrowingTransaction>();
}
