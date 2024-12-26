using SkillMiner.Domain.Shared.Entities;

namespace SkillMiner.Domain.Entities.BackgroundTaskEntity;

public record BackgroundTaskId(Guid Value) : EntityId(Value);