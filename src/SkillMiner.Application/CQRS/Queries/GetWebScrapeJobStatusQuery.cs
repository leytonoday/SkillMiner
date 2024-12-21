using MediatR;
using SkillMiner.Domain.Entities.WebScrapingTaskEntity;

namespace SkillMiner.Application.CQRS.Queries;

public record GetWebScrapeJobStatusQuery(WebScrapingTaskId WebScrapingTaskId) : IRequest<WebScrapingStatus>;

public class GetWebScrapeJobStatusQueryHandler
    (IWebScrapingTaskRepository webScrapingTaskRepository)
    : IRequestHandler<GetWebScrapeJobStatusQuery, WebScrapingStatus>
{
    public async Task<WebScrapingStatus> Handle(GetWebScrapeJobStatusQuery request, CancellationToken cancellationToken)
    {
        WebScrapingTask? webScrapingTask = await webScrapingTaskRepository.GetByIdAsync(request.WebScrapingTaskId, cancellationToken)
            ?? throw new Exception($"WebScrapingTask with Id {request.WebScrapingTaskId} not found.");

        return webScrapingTask.Status;
    }
}
