using SkillMiner.Domain.Shared.Persistence;

namespace SkillMiner.Domain.Entities.BackgroundTaskEntity;

public interface IBackgroundTaskRepository : IRepository<BackgroundTask, BackgroundTaskId>;