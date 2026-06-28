namespace Library.Application.Abstractions.Repositories;

public interface IRepository<TEntity> : IReadRepository<TEntity> where TEntity : class
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    void Update(TEntity entity);

    void Delete(TEntity entity);

    void DeleteRange(IEnumerable<TEntity> entities);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
