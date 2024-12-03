using SkillMiner.Domain.Shared.Persistence;

namespace SkillMiner.Domain.Entities.JobListingEntity;

public interface IJobListingRepository : IRepository<JobListing, JobListingId>;