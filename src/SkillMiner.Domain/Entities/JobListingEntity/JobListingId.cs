using SkillMiner.Domain.Shared.Entities;

namespace SkillMiner.Domain.Entities.JobListingEntity;

public record JobListingId(Guid Value) : EntityId(Value);