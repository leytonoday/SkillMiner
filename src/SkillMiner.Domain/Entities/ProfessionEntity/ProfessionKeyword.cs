using SkillMiner.Domain.Shared.Entities;

namespace SkillMiner.Domain.Entities.ProfessionEntity;

public class ProfessionKeyword : Entity
{
    public ProfessionId ProfessionId { get; set; }
    public Profession Profession { get; set; }

    private ProfessionKeyword() { }

    public string Keyword { get; private set; } = null!;

    public static ProfessionKeyword Create(string keyword, ProfessionId professionId)
    {
        return new ProfessionKeyword()
        {
            Keyword = keyword,
            ProfessionId = professionId,
            CreatedOnUtc = DateTime.UtcNow,
        };
    }

    public override bool IsValid()
    {
        throw new NotImplementedException();
    }
}