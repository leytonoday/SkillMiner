using MediatR;
using SkillMiner.Application.Shared.Models;
using SkillMiner.Domain.Entities.MicrosoftJobListingEntity;

namespace SkillMiner.Application.CQRS.Queries;

public record GetWebScrapedMicrosoftJobsQuery(int PageNumber, int PageSize) : IRequest<PagedResponse<MicrosoftJobListing>>;

public class GetWebScrapedJobsByCompanyQueryHandler
    (IMicrosoftJobListingRepository microsoftJobListingRepository): IRequestHandler<GetWebScrapedMicrosoftJobsQuery, PagedResponse<MicrosoftJobListing>>
{
    public async Task<PagedResponse<MicrosoftJobListing>> Handle(GetWebScrapedMicrosoftJobsQuery request, CancellationToken cancellationToken)
    {
        (IEnumerable<MicrosoftJobListing> jobListings, int totalJobListings) = await microsoftJobListingRepository.GetPageAsync(request.PageNumber, request.PageSize, cancellationToken);

        return new PagedResponse<MicrosoftJobListing>()
        {
            Items = jobListings,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalItemsInPage = jobListings.Count(),
            TotalItems = totalJobListings,
            HasNextPage = ((request.PageNumber + 1) * request.PageSize) <= totalJobListings,
        };
    }
}