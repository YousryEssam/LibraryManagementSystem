namespace Library.Application.DTOs.Borrowing;

public sealed class BorrowingTransactionDetailsDto
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string BookTitle { get; set; } = default!;
    public string BookIsbn { get; set; } = default!;
    public int MemberId { get; set; }
    public string MemberName { get; set; } = default!;
    public string MemberEmail { get; set; } = default!;
    public string ProcessedByUser { get; set; } = default!;
    public DateTime BorrowedAt { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnedAt { get; set; }
    public TransactionStatus Status { get; set; }
    public string? Notes { get; set; }
    public bool IsOverdue { get; set; }
}
