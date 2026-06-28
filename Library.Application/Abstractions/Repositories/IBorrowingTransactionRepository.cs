namespace Library.Application.Abstractions.Repositories;

public interface IBorrowingTransactionRepository : IRepository<BorrowingTransaction>
{
    Task<BorrowingTransaction?> GetWithDetailsAsync(int id, CancellationToken ct = default);
    Task<bool> HasActiveTransactionAsync(int bookId, CancellationToken ct = default);
    IQueryable<BorrowingTransaction> QueryWithDetails();
}
