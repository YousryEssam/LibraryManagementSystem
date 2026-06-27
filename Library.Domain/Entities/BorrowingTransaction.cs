namespace Library.Domain.Entities;

public class BorrowingTransaction : AuditableEntity
{
    public int BookId { get; set; }
    public int MemberId { get; set; }

    /// <summary>
    /// The system user (Librarian/Staff) who processed this transaction.
    /// </summary>
    public int ProcessedByUserId { get; set; }

    public DateTime BorrowedAt { get; set; } = DateTime.UtcNow;

    public DateTime DueDate { get; set; }

    public DateTime? ReturnedAt { get; set; }

    public TransactionStatus Status { get; set; } = TransactionStatus.Active;

    /// <summary>
    /// Optional notes, e.g. book condition on return.
    /// </summary>
    public string? Notes { get; set; }

    // Navigation Properties
    public Book Book { get; set; } = null!;
    public Member Member { get; set; } = null!;
    public SystemUser ProcessedByUser { get; set; } = null!;
}
