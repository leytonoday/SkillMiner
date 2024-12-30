using MediatR;
using SkillMiner.Domain.Entities.ProfessionEntity;

namespace SkillMiner.Application.CQRS.Queries;

public record GetPopularSkillsByProfessionQuery : IRequest<Dictionary<string, List<string>>>;

public class GetPopularSkillsByProfessionQueryHandler
    (IProfessionRepository professionRepository)
    : IRequestHandler<GetPopularSkillsByProfessionQuery, Dictionary<string, List<string>>>
{
    public Task<Dictionary<string, List<string>>> Handle(GetPopularSkillsByProfessionQuery request, CancellationToken cancellationToken)
    {
        // Return the 10 most mentioned keywords for each profession within the last 90 days. That should give the user some indication of the in-demand
        // skills.
        return professionRepository.GetTopProfessionKeywordsByFrequencyAsync(10, TimeSpan.FromDays(90), cancellationToken);
    }
}