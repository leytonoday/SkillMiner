namespace SkillMiner.Domain.Shared.Entities;

public abstract record EntityId(Guid Value)
{
    public override string ToString() => Value.ToString();
}
