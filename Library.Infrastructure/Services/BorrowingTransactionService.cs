using Library.Application.DTOs.Borrowing;

namespace Library.Infrastructure.Services;

public sealed class BorrowingTransactionService : IBorrowingTransactionService
{
    private readonly IBorrowingTransactionRepository _transactionRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IActivityLogService _activityLogService;
    private readonly ICurrentUserService _currentUser;

    public BorrowingTransactionService(
        IBorrowingTransactionRepository transactionRepository,
        IBookRepository bookRepository,
        IMemberRepository memberRepository,
        IActivityLogService activityLogService,
        ICurrentUserService currentUser)
    {
        _transactionRepository = transactionRepository;
        _bookRepository = bookRepository;
        _memberRepository = memberRepository;
        _activityLogService = activityLogService;
        _currentUser = currentUser;
    }

    public async Task<IEnumerable<BorrowingTransactionDto>> GetAllAsync(
        BorrowingFilterQuery filter, CancellationToken ct = default)
    {
        var query = _transactionRepository.QueryWithDetails()
            .Where(t => t.DeletedAt == null);

        if (filter.Status.HasValue)
            query = query.Where(t => t.Status == filter.Status.Value);

        if (filter.MemberId.HasValue)
            query = query.Where(t => t.MemberId == filter.MemberId.Value);

        if (filter.BookId.HasValue)
            query = query.Where(t => t.BookId == filter.BookId.Value);

        return await query
            .OrderByDescending(t => t.BorrowedAt)
            .Select(t => MapToDto(t))
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<BorrowingTransactionDto>> GetOverdueAsync(CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        return await _transactionRepository.QueryWithDetails()
            .Where(t => t.DeletedAt == null
                     && t.Status == TransactionStatus.Active
                     && t.DueDate < now)
            .OrderBy(t => t.DueDate)
            .Select(t => MapToDto(t))
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<BorrowingTransactionDto>> GetByMemberAsync(
        int memberId, CancellationToken ct = default)
    {
        var member = await _memberRepository.GetByIdAsync(memberId, ct)
            ?? throw new KeyNotFoundException("Member not found.");

        return await _transactionRepository.QueryWithDetails()
            .Where(t => t.MemberId == memberId && t.DeletedAt == null)
            .OrderByDescending(t => t.BorrowedAt)
            .Select(t => MapToDto(t))
            .ToListAsync(ct);
    }

    public async Task<BorrowingTransactionDetailsDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var transaction = await _transactionRepository.GetWithDetailsAsync(id, ct)
            ?? throw new KeyNotFoundException("Transaction not found.");

        return MapToDetailsDto(transaction);
    }

    public async Task<int> BorrowAsync(BorrowBookDto request, CancellationToken ct = default)
    {
        if (request.DueDate.ToUniversalTime() <= DateTime.UtcNow)
            throw new ArgumentException("Due date must be in the future.");

        var book = await _bookRepository.GetActiveByIdAsync(request.BookId, ct)
            ?? throw new KeyNotFoundException("Book not found.");

        if (book.Status != BookStatus.Available)
            throw new InvalidOperationException("Book is not available for borrowing.");

        var member = await _memberRepository.GetByIdAsync(request.MemberId, ct)
            ?? throw new KeyNotFoundException("Member not found.");

        if (member.DeletedAt is not null || member.Status != MemberStatus.Active)
            throw new InvalidOperationException("Member account is not active.");

        if (member.MembershipExpiryDate.HasValue && member.MembershipExpiryDate.Value < DateTime.UtcNow)
            throw new InvalidOperationException("Member's membership has expired.");

        if (await _transactionRepository.HasActiveTransactionAsync(request.BookId, ct))
            throw new InvalidOperationException("Book already has an active borrowing transaction.");

        // Update book status
        book.Status = BookStatus.Borrowed;
        book.ModifiedAt = DateTime.UtcNow;
        _bookRepository.Update(book);

        var transaction = new BorrowingTransaction
        {
            BookId = request.BookId,
            MemberId = request.MemberId,
            ProcessedByUserId = _currentUser.UserId,
            BorrowedAt = DateTime.UtcNow,
            DueDate = request.DueDate.ToUniversalTime(),
            Status = TransactionStatus.Active,
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow
        };

        await _transactionRepository.AddAsync(transaction, ct);
        await _transactionRepository.SaveChangesAsync(ct);

        await _activityLogService.LogAsync(
            _currentUser.UserId, "BorrowBook", nameof(BorrowingTransaction),
            transaction.Id,
            $"Book '{book.Title}' borrowed by member '{member.FullName}'.",
            ct: ct);

        return transaction.Id;
    }

    public async Task ReturnAsync(int id, ReturnBookDto request, CancellationToken ct = default)
    {
        var transaction = await _transactionRepository.GetWithDetailsAsync(id, ct)
            ?? throw new KeyNotFoundException("Transaction not found.");

        if (transaction.Status == TransactionStatus.Returned)
            throw new InvalidOperationException("Book has already been returned.");

        var book = await _bookRepository.GetActiveByIdAsync(transaction.BookId, ct)
            ?? throw new KeyNotFoundException("Book not found.");

        // Update transaction
        transaction.ReturnedAt = DateTime.UtcNow;
        transaction.Status = TransactionStatus.Returned;
        transaction.Notes = request.Notes ?? transaction.Notes;
        transaction.ModifiedAt = DateTime.UtcNow;
        _transactionRepository.Update(transaction);

        // Restore book availability
        book.Status = BookStatus.Available;
        book.ModifiedAt = DateTime.UtcNow;
        _bookRepository.Update(book);

        await _transactionRepository.SaveChangesAsync(ct);

        await _activityLogService.LogAsync(
            _currentUser.UserId, "ReturnBook", nameof(BorrowingTransaction),
            transaction.Id,
            $"Book '{book.Title}' returned by member '{transaction.Member.FullName}'.",
            ct: ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var transaction = await _transactionRepository.GetWithDetailsAsync(id, ct)
            ?? throw new KeyNotFoundException("Transaction not found.");

        if (transaction.Status == TransactionStatus.Active)
            throw new InvalidOperationException(
                "Cannot delete an active transaction. Return the book first.");

        transaction.DeletedAt = DateTime.UtcNow;
        transaction.ModifiedAt = DateTime.UtcNow;

        _transactionRepository.Update(transaction);
        await _transactionRepository.SaveChangesAsync(ct);

        await _activityLogService.LogAsync(
            _currentUser.UserId, "DeleteTransaction", nameof(BorrowingTransaction),
            transaction.Id,
            $"Transaction #{transaction.Id} deleted.",
            ct: ct);
    }

    // ── Mappers ────────────────────────────────────────────────────────────────

    private static BorrowingTransactionDto MapToDto(BorrowingTransaction t) => new()
    {
        Id = t.Id,
        BookTitle = t.Book.Title,
        MemberName = t.Member.FullName,
        BorrowedAt = t.BorrowedAt,
        DueDate = t.DueDate,
        ReturnedAt = t.ReturnedAt,
        Status = t.Status
    };

    private static BorrowingTransactionDetailsDto MapToDetailsDto(BorrowingTransaction t) => new()
    {
        Id = t.Id,
        BookId = t.BookId,
        BookTitle = t.Book.Title,
        BookIsbn = t.Book.ISBN,
        MemberId = t.MemberId,
        MemberName = t.Member.FullName,
        MemberEmail = t.Member.Email,
        ProcessedByUser = t.ProcessedByUser.FullName,
        BorrowedAt = t.BorrowedAt,
        DueDate = t.DueDate,
        ReturnedAt = t.ReturnedAt,
        Status = t.Status,
        Notes = t.Notes,
        IsOverdue = t.Status == TransactionStatus.Active && t.DueDate < DateTime.UtcNow
    };
}
