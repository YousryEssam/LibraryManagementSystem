namespace Library.Infrastructure.Repositories;

public sealed class BookRepository : Repository<Book>, IBookRepository
{
    private readonly LibraryDbContext _context;

    public BookRepository(LibraryDbContext context) : base(context)
        => _context = context;

    public async Task<Book?> GetActiveByIdAsync(int id, CancellationToken ct = default)
        => await _context.Books
            .FirstOrDefaultAsync(b => b.Id == id && b.DeletedAt == null, ct);

    public async Task<Book?> GetWithDetailsAsync(int id, CancellationToken ct = default)
        => await _context.Books
            .Include(b => b.Publisher)
            .Include(b => b.Category)
            .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
            .FirstOrDefaultAsync(b => b.Id == id && b.DeletedAt == null, ct);

    public async Task<bool> ExistsByIsbnAsync(string isbn, int? excludeId = null, CancellationToken ct = default)
        => await _context.Books
            .AnyAsync(b => b.ISBN == isbn
                        && b.DeletedAt == null
                        && (excludeId == null || b.Id != excludeId), ct);

    public IQueryable<Book> QueryWithDetails()
        => _context.Books
            .Include(b => b.Publisher)
            .Include(b => b.Category)
            .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
            .AsNoTracking();
}