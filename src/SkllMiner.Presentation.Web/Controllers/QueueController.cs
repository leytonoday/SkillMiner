using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SkillMiner.Application.CQRS.Commands;
using SkillMiner.Application.CQRS.Queries;
using SkillMiner.Application.Shared.Results;

namespace SkillMiner.Presentation.Web.Controllers;

[ApiController]
[Route("/api/{version:apiVersion}/queue")]
[ApiVersion("1.0")]
public class QueueController
    (ISender sender)
    : ControllerBase
{
    [HttpPost("web-scrape-jobs-by-title")]
    public async Task<IActionResult> QueueWebScrapeJobsByTitle([FromQuery(Name = "job-title")] string jobTitle, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new QueueWebScrapeJobsByTitleCommand(jobTitle), cancellationToken);
        return Ok(Result.Success(result));
    }

    [HttpPost("generate-skills-by-job-title")] 
    public async Task<IActionResult> QueueGenerateSkillsByJobTitle(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new QueueGenerateSkillsByJobTitleCommand(), cancellationToken);
        return Ok(Result.Success(result));
    }

    [HttpGet("status/{trackingId}")]
    public async Task<IActionResult> GetStatus([FromRoute] Guid trackingId, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetCommandQueueMessageProcessingStatusQuery(trackingId), cancellationToken);
        return Ok(Result.Success(result));
    }

    [HttpGet("pending-and-processing")]
    public async Task<IActionResult> GetPendingAndProcessing(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetPendingAndProcessingCommandQueueMessagesQuery(), cancellationToken);
        return Ok(Result.Success(result));
    }

    [HttpGet("failed")]
    public async Task<IActionResult> GetFailed(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetFailedCommandQueueMessagesQuery(), cancellationToken);
        return Ok(Result.Success(result));
    }
}
