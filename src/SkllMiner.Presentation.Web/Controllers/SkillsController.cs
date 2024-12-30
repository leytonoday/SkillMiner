using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SkillMiner.Application.CQRS.Queries;
using SkillMiner.Application.Shared.Results;

namespace SkillMiner.Presentation.Web.Controllers;

[ApiController]
[Route("/api/{version:apiVersion}/skills")]
[ApiVersion("1.0")]
public class SkillsController
    (ISender sender)
    : ControllerBase
{
    [HttpGet("popular-skills-by-profession")]
    public async Task<IActionResult> GetPopularSkillsByProfession(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetPopularSkillsByProfessionQuery(), cancellationToken);
        return Ok(Result.Success(result));
    }
}
