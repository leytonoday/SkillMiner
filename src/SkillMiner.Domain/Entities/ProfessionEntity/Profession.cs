using SkillMiner.Domain.Shared.Entities;

namespace SkillMiner.Domain.Entities.ProfessionEntity;

public class Profession : Entity<ProfessionId>
{
    public string Name { get; private set; }

    public readonly IList<ProfessionKeyword> _keywords = [];
    public IReadOnlyCollection<ProfessionKeyword> Keywords => _keywords.AsReadOnly();

    private Profession() { }

    public static Profession Create(string name)
    {
        return new Profession
        {
            Name = name,
            Id = new ProfessionId(Guid.NewGuid())
        };
    }

    public void AddKeywords(IEnumerable<ProfessionKeyword> keywords)
    {
        foreach(var keyword in keywords)
        {
            _keywords.Add(keyword);
        }
    }

    public override bool IsValid()
    {
        throw new NotImplementedException();
    }
}
