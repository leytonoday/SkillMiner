using SkillMiner.Domain.Entities.JobListingEntity;

namespace SkillMiner.Application.Services.WebScraper;

public record JobListingWebScraperInput(string JobTitle);

public interface IJobListingWebScraper : IWebScraper<JobListing, JobListingWebScraperInput>;
