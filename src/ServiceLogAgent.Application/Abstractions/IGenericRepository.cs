using System.Linq.Expressions;

namespace ServiceLogAgent.Application.Abstractions;

public interface IGenericRepository<T> where T : class
{
    IQueryable<T> Query(bool asNoTracking = true);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<int> CountAsync(Func<IQueryable<T>, IQueryable<T>> queryBuilder, CancellationToken cancellationToken = default);
    Task<List<T>> ListAsync(Func<IQueryable<T>, IQueryable<T>> queryBuilder, CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
