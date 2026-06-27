namespace Library.Application.Abstractions.Persistence.Repositories;

public interface IReadRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    IQueryable<TEntity> Query();
}
