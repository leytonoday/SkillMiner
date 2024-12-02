using SkillMiner.Domain.Shared.Entities;
using System.Linq.Expressions;

namespace SkillMiner.Domain.Shared.Persistence;

/// <summary>
/// Represents a repository for working with entities of type <typeparamref name="TEntity"/> that don't have a single primary key.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public interface IRepository<TEntity> where TEntity : class, IEntity
{
    public Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);

    public Task<IEnumerable<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken);

    public Task AddAsync(TEntity entity, CancellationToken cancellationToken);

    public Task RemoveAsync(TEntity entity, CancellationToken cancellationToken);

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);
}

/// <summary>
/// Represents a repository for working with entities of type <typeparamref name="TEntity"/> that have a single primary key of type <typeparamref name="TId"/>.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TId">The type of the entity's primary key.</typeparam>
public interface IRepository<TEntity, in TId> : IRepository<TEntity> where TEntity : class, IEntity<TId> where TId : EntityId
{
    public Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken);
}

