using SkillMiner.Domain.Entities.BackgroundTaskEntity;

namespace SkillMiner.Infrastructure.Persistence.Repositories;

public sealed class BackgroundTaskRepository(DatabaseContext context)
    : Repository<BackgroundTask, DatabaseContext, BackgroundTaskId>(context), IBackgroundTaskRepository;