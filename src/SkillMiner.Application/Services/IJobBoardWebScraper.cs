using SkillMiner.Application.Shared.Results;
using SkillMiner.Domain.Entities.JobListingEntity;

namespace SkillMiner.Application.Services;

public interface IJobBoardWebScraper
{
    public IEnumerable<Result<JobListing>> GetJobListings();
}
