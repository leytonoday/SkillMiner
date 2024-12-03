using Microsoft.EntityFrameworkCore;
using SkillMiner.Domain.Shared.Entities;
using System.Linq.Expressions;
using SkillMiner.Domain.Shared.Persistence;

namespace SkillMiner.Infrastructure.Persistence;

/// <inheritdoc cref="IRepository{TEntity}"/>
public abstract class Repository<TEntity, TDatabaseContext>
    : IRepository<TEntity>
        where TEntity : class, IEntity
        where TDatabaseContext : DbContext
{
    protected readonly TDatabaseContext Context;
    protected readonly DbSet<TEntity> DbSet;

    protected Repository(TDatabaseContext context)
    {
        Context = context;
        DbSet = Context.Set<TEntity>();
    }

    public virtual Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        // We know that the Add here isn't going to be querying the
        // database to create the value for a column, so there's no blocking.
        // So we just use Add to avoid the overhead of AddAsync
        DbSet.Add(entity);
        return Task.CompletedTask;
    }

    public virtual Task RemoveAsync(TEntity entity, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        DbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public virtual Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        DbSet.Update(entity);
        return Task.CompletedTask;
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await GetByConditionAsync(null, cancellationToken);
    }

    public virtual async Task<IEnumerable<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>>? condition, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        IQueryable<TEntity> query = DbSet;
        if (condition != null)
            query = query.Where(condition);

        return await query.ToListAsync(cancellationToken);
    }
}

/// <inheritdoc cref="IRepository{TEntity, TId}"/>
public abstract class Repository<TEntity, TDatabaseContext, TId>(TDatabaseContext context) : Repository<TEntity, TDatabaseContext>(context), IRepository<TEntity, TId>
    where TId : EntityId
    where TEntity : class, IEntity<TId>
    where TDatabaseContext : DbContext
{
    public virtual async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        IQueryable<TEntity> query = DbSet;

        return await query.FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }
}