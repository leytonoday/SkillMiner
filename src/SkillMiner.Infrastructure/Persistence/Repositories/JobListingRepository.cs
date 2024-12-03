using SkillMiner.Domain.Entities.JobListingEntity;

namespace SkillMiner.Infrastructure.Persistence.Repositories;

public sealed class JobListingRepository(DatabaseContext context)
    : Repository<JobListing, DatabaseContext, JobListingId>(context), IJobListingRepository;