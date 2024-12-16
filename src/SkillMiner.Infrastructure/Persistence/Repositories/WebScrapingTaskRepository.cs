using SkillMiner.Domain.Entities.WebScrapingTaskEntity;

namespace SkillMiner.Infrastructure.Persistence.Repositories;

public sealed class WebScrapingTaskRepository(DatabaseContext context)
    : Repository<WebScrapingTask, DatabaseContext, WebScrapingTaskId>(context), IWebScrapingTaskRepository;