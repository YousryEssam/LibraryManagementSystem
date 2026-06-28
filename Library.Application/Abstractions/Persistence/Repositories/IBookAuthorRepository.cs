namespace Library.Application.Abstractions.Persistence.Repositories;

public interface IBookAuthorRepository : IRepository<BookAuthor>
{
    Task<BookAuthor?> GetAsync(int bookId, int authorId, CancellationToken ct = default);
    Task<bool> ExistsAsync(int bookId, int authorId, CancellationToken ct = default);
    Task<int> CountByBookAsync(int bookId, CancellationToken ct = default);
}
