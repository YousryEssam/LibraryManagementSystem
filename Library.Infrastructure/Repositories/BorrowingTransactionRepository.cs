namespace Library.Infrastructure.Repositories;

public sealed class BorrowingTransactionRepository : Repository<BorrowingTransaction>, IBorrowingTransactionRepository
{
    private readonly LibraryDbContext _context;

    public BorrowingTransactionRepository(LibraryDbContext context) : base(context)
        => _context = context;

    public async Task<BorrowingTransaction?> GetWithDetailsAsync(int id, CancellationToken ct = default)
        => await _context.BorrowingTransactions
            .Include(t => t.Book)
            .Include(t => t.Member)
            .Include(t => t.ProcessedByUser)
            .FirstOrDefaultAsync(t => t.Id == id && t.DeletedAt == null, ct);

    public async Task<bool> HasActiveTransactionAsync(int bookId, CancellationToken ct = default)
        => await _context.BorrowingTransactions
            .AnyAsync(t => t.BookId == bookId
                        && t.DeletedAt == null
                        && t.Status == TransactionStatus.Active, ct);

    public IQueryable<BorrowingTransaction> QueryWithDetails()
        => _context.BorrowingTransactions
            .Include(t => t.Book)
            .Include(t => t.Member)
            .Include(t => t.ProcessedByUser)
            .AsNoTracking();
}