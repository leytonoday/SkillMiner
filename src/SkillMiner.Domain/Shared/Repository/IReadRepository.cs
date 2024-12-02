﻿using SkillMiner.Domain.Shared.Entities;
using System.Linq.Expressions;

namespace SkillMiner.Domain.Shared.Repository;

/// <summary>
/// Represents a read-only repository for working with entities of type <typeparamref name="TEntity"/> that don't have a single primary key.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public interface IReadRepository<TEntity> where TEntity : class, IEntity
{
    public Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);

    public Task<IEnumerable<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>> condition);
}

/// <summary>
/// Represents a read-only repository for working with entities of type <typeparamref name="TEntity"/> that have a single primary key of type <typeparamref name="TId"/>.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TId">The type of the entity's primary key.</typeparam>
public interface IReadRepository<TEntity, in TId> : IReadRepository<TEntity> where TEntity : class, IEntity<TId>
{
    public Task<TEntity?> GetByIdAsync(TId id, bool trackChanges, CancellationToken cancellationToken);
}

