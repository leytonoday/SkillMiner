using SkillMiner.Domain.Shared.Entities;

namespace SkillMiner.Domain.Entities.BackgroundTaskEntity;

public class BackgroundTask : Entity<BackgroundTaskId>
{
    public BackgroundTaskStatus Status { get; set; }

    public BackgroundTaskType Type { get; set; }

    public DateTime? StartedOnUtc { get; private set; }
    
    public DateTime? CompletedOnUtc { get; private set; }

    private BackgroundTask() {}

    public override bool IsValid()
    {
        throw new NotImplementedException();
    }

    public static BackgroundTask CreateNew()
    {
        return new BackgroundTask()
        {
            Id = new BackgroundTaskId(Guid.NewGuid()),
            Status = BackgroundTaskStatus.Pending,
            CreatedOnUtc = DateTime.UtcNow,
        };
    }

    public void MarkAsStarted()
    {
        Status = BackgroundTaskStatus.InProgress;
        StartedOnUtc = DateTime.UtcNow;
    }

    public void MarkAsCompleted()
    {
        Status = BackgroundTaskStatus.Completed;
        CompletedOnUtc = DateTime.UtcNow;
    }

    public void MarkAsFailed()
    {
        Status = BackgroundTaskStatus.Failed;
    }
}
