using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillMiner.Domain.Shared.Persistence;

namespace SkillMiner.Infrastructure.Persistence;

public sealed class UnitOfWork(ILogger<UnitOfWork> logger, DatabaseContext databaseContext) : IUnitOfWork
{
    /// <inheritdoc/>
    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await using var dbContextTransaction = await databaseContext.Database.BeginTransactionAsync(cancellationToken);
        logger.LogDebug("Started transaction - {TransactionId}", dbContextTransaction.TransactionId);

        try
        {
            logger.LogDebug("Saving changes - {TransactionId}", dbContextTransaction.TransactionId);
            _ = await databaseContext.SaveChangesAsync(cancellationToken);

            await dbContextTransaction.CommitAsync(cancellationToken);
            logger.LogDebug("Commiting transaction - {TransactionId}", dbContextTransaction.TransactionId);
        }
        catch (Exception e)
        {
            logger.LogError("Commit error - {TransactionId} - Error: {Message}", dbContextTransaction.TransactionId, e.Message);
            logger.LogDebug("Rolling back transaction - {TransactionId}", dbContextTransaction.TransactionId);
            await dbContextTransaction.RollbackAsync(cancellationToken);
        }
    }

    /// <inheritdoc/>
    public bool HasUnsavedChanges()
    {
        return databaseContext.ChangeTracker.Entries().Any(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted);
    }
}
