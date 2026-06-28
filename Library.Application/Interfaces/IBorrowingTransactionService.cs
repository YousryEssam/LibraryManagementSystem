using Library.Application.DTOs.Borrowing;

namespace Library.Application.Interfaces;

public interface IBorrowingTransactionService
{
    Task<IEnumerable<BorrowingTransactionDto>> GetAllAsync(BorrowingFilterQuery filter, CancellationToken ct = default);
    Task<IEnumerable<BorrowingTransactionDto>> GetOverdueAsync(CancellationToken ct = default);
    Task<IEnumerable<BorrowingTransactionDto>> GetByMemberAsync(int memberId, CancellationToken ct = default);
    Task<BorrowingTransactionDetailsDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<int> BorrowAsync(BorrowBookDto request, CancellationToken ct = default);
    Task ReturnAsync(int id, ReturnBookDto request, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}
