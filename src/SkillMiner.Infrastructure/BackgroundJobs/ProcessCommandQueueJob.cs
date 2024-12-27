using Polly.Retry;
using Polly;
using Quartz;
using SkillMiner.Application.Abstractions.CommandQueue;
using SkillMiner.Infrastructure.CommandQueue;
using Microsoft.Extensions.Logging;
using MediatR;
using SkillMiner.Domain.Shared.Persistence;

namespace SkillMiner.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessCommandQueueJob(
    ICommandQueueForConsumer commandQueueForConsumer,
    ISender sender,
    ILogger<ProcessCommandQueueJob> logger,
    IUnitOfWork unitOfWork
    ) : IJob
{
    /// <summary>
    /// Defines a retry policy using Polly for handling exceptions during command execution.
    /// </summary>
    private static readonly AsyncRetryPolicy _retryPolicy = Policy
        .Handle<Exception>()
        .WaitAndRetryAsync(
            3,
            attempt => TimeSpan.FromMilliseconds(100 * attempt)
        );

    public async Task Execute(IJobExecutionContext context)
    {
        List<CommandQueueMessage> messages = await commandQueueForConsumer.ListPendingAsync(context.CancellationToken);
        if (messages.Count == 0)
        {
            return;
        }

        logger.LogDebug("Found {messagesCount} pending messages in queue.", messages.Count);

        foreach (var message in messages)
        {
            logger.LogInformation($"Processing queue message: {message.Id} {message.Type} {message.Data}");

            await commandQueueForConsumer.MarkStartedAsync(message, context.CancellationToken);
            await unitOfWork.CommitAsync(context.CancellationToken);

            PolicyResult result = await _retryPolicy.ExecuteAndCaptureAsync(() => sender.Send(CommandQueueMessage.ToRequest(message), context.CancellationToken));

            if (result.Outcome == OutcomeType.Failure)
            {
                // Log the exception, mark message with error, and potentially notify
                logger.LogError(result.FinalException, "Cannot send the command for QueuedMessage with Id {messageId}", message.Id);

                await commandQueueForConsumer.MarkFailedAsync(message, result.FinalException?.ToString() ?? "Unknwon error", context.CancellationToken);
                continue;
            }

            await commandQueueForConsumer.MarkProcessedAsync(message, context.CancellationToken);
        }

        await unitOfWork.CommitAsync(context.CancellationToken);
    }
}
