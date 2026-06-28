namespace Library.Application.Abstractions.Persistence.Repositories;

public interface IPublisherRepository : IRepository<Publisher>
{
    Task<bool> ExistsByNameAsync(string name, int? excludeId = null, CancellationToken ct = default);
    Task<bool> HasActiveBooksAsync(int id, CancellationToken ct = default);
}
