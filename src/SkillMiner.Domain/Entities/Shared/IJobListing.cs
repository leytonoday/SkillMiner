namespace SkillMiner.Domain.Entities.Shared;

public interface IJobListing
{
    /// <summary>
    /// The original URL of the webpage from which the job listing was scraped.
    /// </summary>
    public string Url { get; }

    /// <summary>
    /// Indicates if this job listing has already been analysed and it's keywords have been extracted.
    /// </summary>
    public bool HadKeywordsExtracted { get; }
}
