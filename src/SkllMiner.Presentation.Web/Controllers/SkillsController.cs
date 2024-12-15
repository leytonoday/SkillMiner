using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SkillMiner.Application.CQRS.JobListingEntity.Commands;
using SkillMiner.Application.Shared.Results;

namespace SkillMiner.Presentation.Web.Controllers;

[ApiController]
[Route("/api/{version:apiVersion}/skills")]
[ApiVersion("1.0")]
public class SkillsController
    (ISender sender)
    : ControllerBase
{
    [HttpPost("generate-by-job-title/{jobTitle}")]
    public async Task<IActionResult> GenerateSkillsByJobTitle([FromRoute] string jobTitle, CancellationToken cancellationToken)
    {
        await sender.Send(new QueueGenerateSkillsByJobTitleCommand(jobTitle), cancellationToken);
        return Ok(Result.Success());
    }
}
