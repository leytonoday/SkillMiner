using SkillMiner.Application.Abstractions.CommandQueue;
using SkillMiner.Application.Services.WebScraper;
using SkillMiner.Domain.Entities.MicrosoftJobListingEntity;
using SkillMiner.Domain.Shared.Persistence;

namespace SkillMiner.Application.CQRS.Queue;

public sealed record WebScrapeJobsByTitleQueuedCommand(string JobTitle) : QueuedCommand;

public sealed class WebScrapeJobsByTitleQueuedCommandHandler
    (IJobListingWebScraper<MicrosoftJobListing> microsoftJobListingWebScaper,
    IMicrosoftJobListingRepository microsoftJobListingRepository,
    IUnitOfWork unitOfWork)
    : IQueuedCommandHandler<WebScrapeJobsByTitleQueuedCommand>
{
    public async Task Handle(WebScrapeJobsByTitleQueuedCommand request, CancellationToken cancellationToken)
    {
        var webScraperInput = new JobListingWebScraperInput(request.JobTitle);

        var result = await microsoftJobListingWebScaper.ScrapeAsync(webScraperInput, cancellationToken);

        // Succeeded and there are new job listings. Add to repo.
        foreach (var jobListing in result.Data!)
        {
            await microsoftJobListingRepository.AddAsync(jobListing, cancellationToken);
        }
        
        await unitOfWork.CommitAsync(cancellationToken);
    }
}