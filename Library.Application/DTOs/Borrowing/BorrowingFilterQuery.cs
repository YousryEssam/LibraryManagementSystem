namespace Library.Application.DTOs.Borrowing;

public sealed class BorrowingFilterQuery
{
    public TransactionStatus? Status { get; set; }
    public int? MemberId { get; set; }
    public int? BookId { get; set; }
}
