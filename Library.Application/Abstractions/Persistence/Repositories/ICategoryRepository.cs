namespace Library.Application.Abstractions.Persistence.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<Category?> GetWithDetailsAsync(int id, CancellationToken ct = default);
    Task<bool> ExistsByNameAsync(string name, int? excludeId = null, CancellationToken ct = default);
    Task<bool> HasChildrenAsync(int id, CancellationToken ct = default);
    Task<bool> HasActiveBooksAsync(int id, CancellationToken ct = default);
}
