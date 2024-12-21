using SkillMiner.Domain.Shared.Entities;

namespace SkillMiner.Domain.Entities.WebScrapingTaskEntity;

public class WebScrapingTask: Entity<WebScrapingTaskId>
{
    public WebScrapingStatus Status { get; set; }

    public WebScrapingType Type { get; set; }

    public DateTime? StartedOnUtc { get; private set; }
    
    public DateTime? CompletedOnUtc { get; private set; }

    private WebScrapingTask() {}

    public override bool IsValid()
    {
        throw new NotImplementedException();
    }

    public static WebScrapingTask CreateNew()
    {
        return new WebScrapingTask()
        {
            Id = new WebScrapingTaskId(Guid.NewGuid()),
            Status = WebScrapingStatus.Pending,
            CreatedOnUtc = DateTime.UtcNow,
        };
    }

    public void MarkAsStarted()
    {
        Status = WebScrapingStatus.InProgress;
        StartedOnUtc = DateTime.UtcNow;
    }

    public void MarkAsCompleted()
    {
        Status = WebScrapingStatus.Completed;
        CompletedOnUtc = DateTime.UtcNow;
    }

    public void MarkAsFailed()
    {
        Status = WebScrapingStatus.Failed;
    }
}
