using SkillMiner.Application.Shared.Results;

namespace SkillMiner.Application.Services.WebScraper;

public interface IWebScraper<TSubject, TInput>
    where TSubject : class
    where TInput : class
{
    public Task<Result<IEnumerable<TSubject>>> ScrapeAsync(TInput input, CancellationToken cancellationToken);
}
