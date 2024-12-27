namespace SkillMiner.Application.Services.WebScraper;

public record JobListingWebScraperInput(string JobTitle);

public interface IJobListingWebScraper<TReturn> : IWebScraper<TReturn, JobListingWebScraperInput> where TReturn : class;
