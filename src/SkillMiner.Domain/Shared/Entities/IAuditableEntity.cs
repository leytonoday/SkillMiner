namespace SkillMiner.Domain.Shared.Entities;

/// <summary>
/// Represents an auditable entity with date created and date updated properties.
/// </summary>
public interface IAuditableEntity
{
    /// <summary>
    /// The date and time when the entity was created.
    /// </summary>
    public DateTime CreatedOnUtc { get; init; }

    /// <summary>
    /// The date and time when the entity was last updated.
    /// </summary>
    public DateTime? UpdatedOnUtc { get; init; }
}