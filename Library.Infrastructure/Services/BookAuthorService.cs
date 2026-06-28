using Library.Application.DTOs.Books;

namespace Library.Infrastructure.Services;

public sealed class BookAuthorService : IBookAuthorService
{
    private readonly IBookRepository _bookRepository;
    private readonly IBookAuthorRepository _bookAuthorRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IActivityLogService _activityLogService;
    private readonly ICurrentUserService _currentUser;

    public BookAuthorService(
        IBookRepository bookRepository,
        IBookAuthorRepository bookAuthorRepository,
        IAuthorRepository authorRepository,
        IActivityLogService activityLogService,
        ICurrentUserService currentUser)
    {
        _bookRepository = bookRepository;
        _bookAuthorRepository = bookAuthorRepository;
        _authorRepository = authorRepository;
        _activityLogService = activityLogService;
        _currentUser = currentUser;
    }

    public async Task AddAsync(int bookId, BookAuthorEntryDto request, CancellationToken ct = default)
    {
        await EnsureBookActiveAsync(bookId, ct);

        var author = await _authorRepository.Query()
            .AnyAsync(a => a.Id == request.AuthorId && a.DeletedAt == null, ct);
        if (!author)
            throw new KeyNotFoundException($"Author with id {request.AuthorId} not found.");

        if (await _bookAuthorRepository.ExistsAsync(bookId, request.AuthorId, ct))
            throw new InvalidOperationException("This author is already linked to the book.");

        await _bookAuthorRepository.AddAsync(new BookAuthor
        {
            BookId = bookId,
            AuthorId = request.AuthorId,
            Role = request.Role
        }, ct);

        await _bookAuthorRepository.SaveChangesAsync(ct);

        await _activityLogService.LogAsync(
            _currentUser.UserId, "AddBookAuthor", nameof(BookAuthor), bookId,
            $"Author {request.AuthorId} added to book {bookId}.", ct: ct);
    }

    public async Task UpdateRoleAsync(int bookId, int authorId, UpdateBookAuthorRoleDto request, CancellationToken ct = default)
    {
        await EnsureBookActiveAsync(bookId, ct);

        var link = await _bookAuthorRepository.GetAsync(bookId, authorId, ct)
            ?? throw new KeyNotFoundException("Author is not linked to this book.");

        link.Role = request.Role;
        _bookAuthorRepository.Update(link);
        await _bookAuthorRepository.SaveChangesAsync(ct);

        await _activityLogService.LogAsync(
            _currentUser.UserId, "UpdateBookAuthorRole", nameof(BookAuthor), bookId,
            $"Author {authorId} role updated to '{request.Role}' on book {bookId}.", ct: ct);
    }

    public async Task RemoveAsync(int bookId, int authorId, CancellationToken ct = default)
    {
        await EnsureBookActiveAsync(bookId, ct);

        var link = await _bookAuthorRepository.GetAsync(bookId, authorId, ct)
            ?? throw new KeyNotFoundException("Author is not linked to this book.");

        if (await _bookAuthorRepository.CountByBookAsync(bookId, ct) <= 1)
            throw new InvalidOperationException("A book must have at least one author.");

        _bookAuthorRepository.Delete(link);
        await _bookAuthorRepository.SaveChangesAsync(ct);

        await _activityLogService.LogAsync(
            _currentUser.UserId, "RemoveBookAuthor", nameof(BookAuthor), bookId,
            $"Author {authorId} removed from book {bookId}.", ct: ct);
    }

    // ── Helpers ────────────────────────────────────────────────────────────────

    private async Task EnsureBookActiveAsync(int bookId, CancellationToken ct)
    {
        var exists = await _bookRepository.Query()
            .AnyAsync(b => b.Id == bookId && b.DeletedAt == null, ct);
        if (!exists)
            throw new KeyNotFoundException("Book not found.");
    }
}
