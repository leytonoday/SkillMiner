using SkillMiner.Domain.Shared.Persistence;

namespace SkillMiner.Domain.Entities.ProfessionEntity;

public interface IProfessionRepository : IRepository<Profession, ProfessionId>
{
    Task<Profession?> GetByNameAsync(string name, CancellationToken cancellationToken);

    Task<Dictionary<string, List<string>>> GetTopProfessionKeywordsByFrequencyAsync(int limit, TimeSpan withinDate, CancellationToken cancellationToken);
}