using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ServiceLogAgent.Application.Abstractions;
using ServiceLogAgent.Persistence.Context;

namespace ServiceLogAgent.Persistence.Repositories;

public class GenericRepository<T>(ServiceLogDbContext context) : IGenericRepository<T> where T : class
{
    public IQueryable<T> Query(bool asNoTracking = true)
    {
        var query = context.Set<T>().AsQueryable();
        return asNoTracking ? query.AsNoTracking() : query;
    }

    public Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return context.Set<T>().FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public Task<int> CountAsync(Func<IQueryable<T>, IQueryable<T>> queryBuilder, CancellationToken cancellationToken = default)
    {
        var query = queryBuilder(Query());
        return query.CountAsync(cancellationToken);
    }

    public Task<List<T>> ListAsync(Func<IQueryable<T>, IQueryable<T>> queryBuilder, CancellationToken cancellationToken = default)
    {
        var query = queryBuilder(Query());
        return query.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await context.Set<T>().AddAsync(entity, cancellationToken);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return context.SaveChangesAsync(cancellationToken);
    }
}
