using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SkillMiner.Application.CQRS.Commands;
using SkillMiner.Application.CQRS.Queries;
using SkillMiner.Application.Shared.Results;

namespace SkillMiner.Presentation.Web.Controllers;

[ApiController]
[Route("/api/{version:apiVersion}/jobs")]
[ApiVersion("1.0")]
public class JobsController
    (ISender sender)
    : ControllerBase
{
    [HttpPost("queue-web-scrape-jobs-by-title")]
    public async Task<IActionResult> QueueWebScrapeJobsByTitle([FromQuery(Name = "job-title")] string jobTitle, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new QueueWebScrapeJobsByTitleCommand(jobTitle), cancellationToken);
        return Ok(Result.Success(result));
    }

    [HttpGet("get-web-scrape-job-status/{webScrapeTaskId}")]
    public async Task<IActionResult> GetWebScrapeJobStatus(Guid webScrapeTaskId, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetWebScrapeJobStatusQuery(new Domain.Entities.WebScrapingTaskEntity.WebScrapingTaskId(webScrapeTaskId)), cancellationToken);
        return Ok(Result.Success(result));
    }

    [HttpGet("get-web-scraped-microsoft-jobs")]
    public async Task<IActionResult> GetWebScrapedMicrosoftJobs([FromQuery(Name = "page-number")] int pageNumber, [FromQuery(Name = "page-size")] int pageSize, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetWebScrapedMicrosoftJobsQuery(pageNumber, pageSize), cancellationToken);
        return Ok(Result.Success(result));
    }
}
 