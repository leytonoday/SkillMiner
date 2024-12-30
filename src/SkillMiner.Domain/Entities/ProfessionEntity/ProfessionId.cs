using SkillMiner.Domain.Shared.Entities;

namespace SkillMiner.Domain.Entities.ProfessionEntity;

public record ProfessionId(Guid Value) : EntityId(Value);