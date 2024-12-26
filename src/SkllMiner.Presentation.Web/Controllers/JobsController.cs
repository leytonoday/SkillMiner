using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
    [HttpGet("web-scraped-jobs/microsoft")]
    public async Task<IActionResult> GetWebScrapedMicrosoftJobs(
        [FromQuery(Name = "page-number")] int pageNumber, 
        [FromQuery(Name = "page-size")] int pageSize, 
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetWebScrapedMicrosoftJobsQuery(pageNumber, pageSize), cancellationToken);
        return Ok(Result.Success(result));
    }
}
