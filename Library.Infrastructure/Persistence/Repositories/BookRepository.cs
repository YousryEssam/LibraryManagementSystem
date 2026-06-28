namespace Library.Infrastructure.Persistence.Repositories;

public sealed class BookRepository : Repository<Book>, IBookRepository
{
    private readonly LibraryDbContext _context;

    public BookRepository(LibraryDbContext context) : base(context)
        => _context = context;

    public async Task<Book?> GetActiveByIdAsync(int id, CancellationToken ct = default)
        => await _context.Books
            .FirstOrDefaultAsync(b => b.Id == id && b.DeletedAt == null, ct);
}
