namespace Library.Infrastructure.Repositories;

public sealed class PublisherRepository : Repository<Publisher>, IPublisherRepository
{
    private readonly LibraryDbContext _context;

    public PublisherRepository(LibraryDbContext context) : base(context)
        => _context = context;

    public async Task<bool> ExistsByNameAsync(string name, int? excludeId = null, CancellationToken ct = default)
        => await _context.Publishers
            .AnyAsync(p => p.Name == name
                        && p.DeletedAt == null
                        && (excludeId == null || p.Id != excludeId), ct);

    public async Task<bool> HasActiveBooksAsync(int id, CancellationToken ct = default)
        => await _context.Books
            .AnyAsync(b => b.PublisherId == id && b.DeletedAt == null, ct);
}
