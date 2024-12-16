using SkillMiner.Application.Shared.Results;

namespace SkillMiner.Application.Services.WebScraper;

public interface IWebScraper<TSubject, TInput>
    where TSubject : class
    where TInput : class
{
    public Task<IEnumerable<Result<TSubject>>> ScrapeAsync(TInput input);
}
