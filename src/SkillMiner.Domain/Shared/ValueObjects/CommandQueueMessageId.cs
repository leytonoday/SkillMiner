using SkillMiner.Domain.Shared.Entities;

namespace SkillMiner.Domain.Shared.ValueObjects;

public record CommandQueueMessageId(Guid Value) : EntityId(Value);
