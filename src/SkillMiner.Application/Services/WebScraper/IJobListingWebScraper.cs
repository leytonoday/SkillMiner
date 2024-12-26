using SkillMiner.Domain.Entities.BackgroundTaskEntity;

namespace SkillMiner.Application.Services.WebScraper;

public record JobListingWebScraperInput(BackgroundTaskId BackgroundTaskId, string JobTitle);

public interface IJobListingWebScraper<TReturn> : IWebScraper<TReturn, JobListingWebScraperInput> where TReturn : class;
