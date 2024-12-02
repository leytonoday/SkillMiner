using SkillMiner.Domain.Shared.Entities;

namespace SkillMiner.Domain.Shared.Repository;

/// <summary>
/// Represents a write-only repository for working with entities of type <typeparamref name="TEntity"/> that don't have a single primary key.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public interface IWriteRepository<in TEntity> where TEntity : class, IEntity
{
    public Task AddAsync(TEntity entity, CancellationToken cancellationToken);

    public Task RemoveAsync(TEntity entity, CancellationToken cancellationToken);

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);
}