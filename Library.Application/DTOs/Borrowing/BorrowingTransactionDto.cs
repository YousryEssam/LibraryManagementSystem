namespace Library.Application.DTOs.Borrowing;

public sealed class BorrowingTransactionDto
{
    public int Id { get; set; }
    public string BookTitle { get; set; } = default!;
    public string MemberName { get; set; } = default!;
    public DateTime BorrowedAt { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnedAt { get; set; }
    public TransactionStatus Status { get; set; }
}
