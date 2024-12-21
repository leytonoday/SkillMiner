using SkillMiner.Domain.Entities.WebScrapingTaskEntity;

namespace SkillMiner.Application.Services.WebScraper;

public record JobListingWebScraperInput(WebScrapingTaskId WebScrapingTaskId, string JobTitle);

public interface IJobListingWebScraper<TReturn> : IWebScraper<TReturn, JobListingWebScraperInput> where TReturn : class;
