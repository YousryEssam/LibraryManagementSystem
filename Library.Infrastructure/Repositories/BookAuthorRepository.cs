namespace Library.Infrastructure.Repositories;

public sealed class BookAuthorRepository : Repository<BookAuthor>, IBookAuthorRepository
{
    private readonly LibraryDbContext _context;

    public BookAuthorRepository(LibraryDbContext context) : base(context)
        => _context = context;

    public async Task<BookAuthor?> GetAsync(int bookId, int authorId, CancellationToken ct = default)
        => await _context.BookAuthors
            .FirstOrDefaultAsync(ba => ba.BookId == bookId && ba.AuthorId == authorId, ct);

    public async Task<bool> ExistsAsync(int bookId, int authorId, CancellationToken ct = default)
        => await _context.BookAuthors
            .AnyAsync(ba => ba.BookId == bookId && ba.AuthorId == authorId, ct);

    public async Task<int> CountByBookAsync(int bookId, CancellationToken ct = default)
        => await _context.BookAuthors
            .CountAsync(ba => ba.BookId == bookId, ct);
}
