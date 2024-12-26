namespace SkillMiner.Domain.Entities.BackgroundTaskEntity;

public sealed class BackgroundTaskException(string Message) : Exception(Message);