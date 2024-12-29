using SkillMiner.Application.Abstractions.CommandQueue;
using SkillMiner.Application.Services.WebScraper;
using SkillMiner.Domain.Entities.MicrosoftJobListingEntity;
using SkillMiner.Domain.Shared.Persistence;

namespace SkillMiner.Application.CQRS.Queue;

public sealed record WebScrapeJobsByProfessionQueuedCommand(string Profession) : QueuedCommand;

public sealed class WebScrapeJobsByProfessionQueuedCommandHandler
    (IJobListingWebScraper<MicrosoftJobListing> microsoftJobListingWebScaper,
    IMicrosoftJobListingRepository microsoftJobListingRepository,
    IUnitOfWork unitOfWork)
    : IQueuedCommandHandler<WebScrapeJobsByProfessionQueuedCommand>
{
    public async Task Handle(WebScrapeJobsByProfessionQueuedCommand request, CancellationToken cancellationToken)
    {
        var webScraperInput = new JobListingWebScraperInput(request.Profession);

        var result = await microsoftJobListingWebScaper.ScrapeAsync(webScraperInput, cancellationToken);

        // Succeeded and there are new job listings. Add to repo.
        foreach (var jobListing in result.Data!)
        {
            await microsoftJobListingRepository.AddAsync(jobListing, cancellationToken);
        }
        
        await unitOfWork.CommitAsync(cancellationToken);
    }
}