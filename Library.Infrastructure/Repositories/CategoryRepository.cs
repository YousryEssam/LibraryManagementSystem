namespace Library.Infrastructure.Repositories;

public sealed class CategoryRepository : Repository<Category>, ICategoryRepository
{
    private readonly LibraryDbContext _context;

    public CategoryRepository(LibraryDbContext context) : base(context)
        => _context = context;

    public async Task<Category?> GetWithDetailsAsync(int id, CancellationToken ct = default)
        => await _context.Categories
            .Include(c => c.ParentCategory)
            .Include(c => c.SubCategories.Where(s => s.DeletedAt == null))
            .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null, ct);

    public async Task<bool> ExistsByNameAsync(string name, int? excludeId = null, CancellationToken ct = default)
        => await _context.Categories
            .AnyAsync(c => c.Name == name
                        && c.DeletedAt == null
                        && (excludeId == null || c.Id != excludeId), ct);

    public async Task<bool> HasChildrenAsync(int id, CancellationToken ct = default)
        => await _context.Categories
            .AnyAsync(c => c.ParentCategoryId == id && c.DeletedAt == null, ct);

    public async Task<bool> HasActiveBooksAsync(int id, CancellationToken ct = default)
        => await _context.Books
            .AnyAsync(b => b.CategoryId == id && b.DeletedAt == null, ct);
}
