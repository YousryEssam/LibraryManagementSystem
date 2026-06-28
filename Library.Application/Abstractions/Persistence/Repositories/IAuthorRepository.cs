namespace Library.Application.Abstractions.Persistence.Repositories;

public interface IAuthorRepository : IRepository<Author>
{
    Task<Author?> GetWithBooksAsync(int id, CancellationToken ct = default);
    Task<bool> HasActiveBooksAsync(int id, CancellationToken ct = default);
}
