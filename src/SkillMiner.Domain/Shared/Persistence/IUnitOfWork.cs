namespace SkillMiner.Domain.Shared.Persistence;

/// <summary>
/// Represents a unit of work for managing database transactions.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Commits the changes made within the unit of work to the underlying database.
    /// </summary>
    public Task CommitAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Determines if there have been any unsaved changes that need saving.
    /// </summary>
    /// <returns><c>true</c> if changes have been made and a database commit is required, <c>false</c> otherwise.</returns>
    public bool HasUnsavedChanges();
}

