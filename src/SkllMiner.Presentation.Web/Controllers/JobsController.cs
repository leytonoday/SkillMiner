using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SkillMiner.Application.CQRS.JobListingEntity.Commands;
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
}
 