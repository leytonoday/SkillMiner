using Newtonsoft.Json;

namespace SkillMiner.Application.Abstractions.CommandQueue;

/// <summary>
/// Represents a <see cref="QueuedCommand"> that has been serialised and stored in a queue system. 
/// </summary>
public class CommandQueueMessage
{
    /// <summary>
    /// The Id of the message.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// The full name of the message type.
    /// </summary>
    public string Type { get; private set; } = null!;

    /// <summary>
    /// The message data that has been serialised to JSON.
    /// </summary>
    public string Data { get; private set; } = null!;

    /// <summary>
    /// The date and time that the queued command was serialized and added to the queue.
    /// </summary>
    public DateTime CreatedOnUtc { get; private set; }

    /// <summary>
    /// The date and time that the queued command message was processed successfully. This could mean either it was published successfully, or unsuccessfully. Either way, it was processed.
    /// </summary>
    public DateTime? ProcessedOnUtc { get; private set; }

    /// <summary>
    /// The error thrown when attempting to execute the queued message and publish it's <see cref="QueuedCommand"/>.
    /// </summary>
    public string? Error { get; private set; }

    /// <summary>
    /// Marks the <see cref="CommandQueueMessage"/> as processed, indicating that the <see cref="QueuedCommand"/> that it stores has been successfully executed.
    /// </summary>
    public void MarkProcessed()
    {
        ProcessedOnUtc = DateTime.UtcNow; // TODO - Get date from IDateTimeProvider
    }

    /// <summary>
    /// Sets the error value on the <see cref="CommandQueueMessage"/> indicating that the queued command could not be executed for whatever reason. Think of setting this column to anything other than NULL the same as 
    /// moving the message to a dead-letter-queue.
    /// </summary>
    /// <param name="error">The error message from the <see cref="Exception"/> that was thrown when attempting to run the command.</param>
    public void SetError(string error)
    {
        Error = error;
    }

    public static CommandQueueMessage CreateFrom<TCommand>(TCommand command) where TCommand : QueuedCommand
    {
        return new()
        {
            Id = Guid.NewGuid(),
            Type = command.GetType().Name,
            CreatedOnUtc = DateTime.UtcNow,
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
}
