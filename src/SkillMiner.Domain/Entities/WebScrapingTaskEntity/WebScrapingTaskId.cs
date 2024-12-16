using SkillMiner.Domain.Shared.Entities;

namespace SkillMiner.Domain.Entities.WebScrapingTaskEntity;

public record WebScrapingTaskId(Guid Value) : EntityId(Value);