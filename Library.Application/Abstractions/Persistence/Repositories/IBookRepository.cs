namespace Library.Application.Abstractions.Persistence.Repositories;

public interface IBookRepository : IRepository<Book>
{
    Task<Book?> GetActiveByIdAsync(int id, CancellationToken ct = default);
}
