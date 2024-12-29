namespace SkillMiner.Application.Services.WebScraper;

public record JobListingWebScraperInput(string Profession);

public interface IJobListingWebScraper<TReturn> : IWebScraper<TReturn, JobListingWebScraperInput> where TReturn : class;
