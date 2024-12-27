using Newtonsoft.Json;
using SkillMiner.Domain.Shared.Entities;
using SkillMiner.Domain.Shared.ValueObjects;

namespace SkillMiner.Application.Abstractions.CommandQueue;

public enum ProcessingStatus
{
    Pending,
    InProgress,
    Processed,
    Failed
}

/// <summary>
/// Represents a <see cref="QueuedCommand"> that has been serialised and stored in a queue system. 
/// </summary>
public class CommandQueueMessage : Entity<CommandQueueMessageId>
{
    /// <summary>
    /// An ID used solely for external systems or users to track the status of the command queue message. This can be used to see
    /// if it's been processed, has failed, or is in a processing state.
    /// </summary>
    public Guid TrackingId { get; private set; }

    /// <summary>
    /// The full name of the message type.
    /// </summary>
    public string Type { get; private set; } = null!;

    /// <summary>
    /// The message data that has been serialised to JSON.
    /// </summary>
    public string Data { get; private set; } = null!;

    /// <summary>
    /// Indicates the processing status and progress of the <see cref="QueuedCommand"/> within this <see cref="CommandQueueMessage"/>.
    /// </summary>
    public ProcessingStatus ProcessingStatus { get; private set; }

    public DateTime? StartedOnUtc { get; private set; }

    public void MarkStarted()
    {
        ProcessingStatus = ProcessingStatus.InProgress;
        StartedOnUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// The date and time that the queued command message was processed successfully. This could mean either it was published successfully, or unsuccessfully. Either way, it was processed.
    /// </summary>
    public DateTime? ProcessedOnUtc { get; private set; }

    /// <summary>
    /// Marks the <see cref="CommandQueueMessage"/> as processed, indicating that the <see cref="QueuedCommand"/> that it stores has been successfully executed.
    /// </summary>
    public void MarkProcessed()
    {
        ProcessedOnUtc = DateTime.UtcNow; // TODO - Get date from IDateTimeProvider
        ProcessingStatus = ProcessingStatus.Processed;
    }

    /// <summary>
    /// The error thrown when attempting to execute the queued message and publish it's <see cref="QueuedCommand"/>.
    /// </summary>
    public string? Error { get; private set; }

    /// <summary>
    /// Sets the error value on the <see cref="CommandQueueMessage"/> indicating that the queued command could not be executed for whatever reason. Think of setting this column to anything other than NULL the same as 
    /// moving the message to a dead-letter-queue.
    /// </summary>
    /// <param name="error">The error message from the <see cref="Exception"/> that was thrown when attempting to run the command.</param>
    public void SetError(string error)
    {
        Error = error;
        ProcessingStatus = ProcessingStatus.Failed;
    }

    public static CommandQueueMessage CreateFrom<TCommand>(TCommand command) where TCommand : QueuedCommand
    {
        return new()
        {
            Id = new CommandQueueMessageId(Guid.NewGuid()),
            Type = command.GetType().Name,
            CreatedOnUtc = DateTime.UtcNow,
            TrackingId = Guid.NewGuid(),
            ProcessingStatus = ProcessingStatus.Pending,
            Data = JsonConvert.SerializeObject(
                command,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                })
        };
    }

    /// <summary>
    /// Deserialises an <see cref="CommandQueueMessage"/> into an <see cref="QueuedCommand"/>.
    /// </summary>
    /// <param name="commandQueueMessage">The <see cref="CommandQueueMessage"/> to convert.</param>
    /// <returns>The deserialised <see cref="QueuedCommand"/>.</returns>
    public static QueuedCommand? ToRequest(CommandQueueMessage commandQueueMessage)
    {
        return JsonConvert
            .DeserializeObject<QueuedCommand>(
                commandQueueMessage.Data,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });
    }

    public override bool IsValid()
    {
        throw new NotImplementedException();
    }
}
