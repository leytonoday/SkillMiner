using Polly.Retry;
using Polly;
using Quartz;
using System.Threading;
using SkillMiner.Application.Abstractions.CommandQueue;
using SkillMiner.Infrastructure.CommandQueue;
using Microsoft.Extensions.Logging;
using MediatR;
using SkillMiner.Domain.Shared.Persistence;
using SkillMiner.Infrastructure.Persistence;

namespace SkillMiner.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessCommandQueueJob(
    ICommandQueueReader commandQueueReader,
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
        List<CommandQueueMessage> messages = await commandQueueReader.ListPendingAsync(context.CancellationToken);
        if (messages.Count == 0)
        {
            return;
        }

        logger.LogDebug("Found {messagesCount} pending messages in queue.", messages.Count);

        foreach (var message in messages)
        {
            logger.LogInformation($"Processing queue message: {message.Id} {message.Type} {message.Data}");

            PolicyResult result = await _retryPolicy.ExecuteAndCaptureAsync(() => sender.Send(CommandQueueMessage.ToRequest(message), context.CancellationToken));

            if (result.Outcome == OutcomeType.Failure)
            {
                // Log the exception, mark message with error, and potentially notify
                logger.LogError(result.FinalException, "Cannot send the command for QueuedMessage with Id {messageId}", message.Id);

                await commandQueueReader.MarkFailedAsync(message, result.FinalException?.ToString() ?? "Unknwon error", context.CancellationToken);
                continue;
            }

            await commandQueueReader.MarkProcessedAsync(message, context.CancellationToken);
        }

        await unitOfWork.CommitAsync(context.CancellationToken);
    }
}
