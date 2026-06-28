namespace Library.Infrastructure.Persistence.Repositories;

public sealed class AuthorRepository : Repository<Author>, IAuthorRepository
{
    private readonly LibraryDbContext _context;

    public AuthorRepository(LibraryDbContext context) : base(context)
        => _context = context;

    public async Task<Author?> GetWithBooksAsync(int id, CancellationToken ct = default)
        => await _context.Authors
            .Include(a => a.BookAuthors)
            .FirstOrDefaultAsync(a => a.Id == id && a.DeletedAt == null, ct);

    public async Task<bool> HasActiveBooksAsync(int id, CancellationToken ct = default)
        => await _context.BookAuthors
            .AnyAsync(ba => ba.AuthorId == id && ba.Book.DeletedAt == null, ct);
}
