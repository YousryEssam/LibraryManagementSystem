namespace Library.Application.Abstractions.Repositories;

public interface IBookRepository : IRepository<Book>
{
    Task<Book?> GetActiveByIdAsync(int id, CancellationToken ct = default);
    Task<Book?> GetWithDetailsAsync(int id, CancellationToken ct = default);
    Task<bool> ExistsByIsbnAsync(string isbn, int? excludeId = null, CancellationToken ct = default);
    IQueryable<Book> QueryWithDetails();
}
