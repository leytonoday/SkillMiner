using SkillMiner.Domain.Entities.BackgroundTaskEntity;
using SkillMiner.Domain.Shared.Entities;

namespace SkillMiner.Domain.Entities.MicrosoftJobListingEntity;

/// <summary>
/// Job Listing information webscraped from the Microsoft Careers Job Board.
/// </summary>
public class MicrosoftJobListing : Entity<MicrosoftJobListingId>
{
    /// <summary>
    /// The URL of the Webpage that the job listing from scraped from.
    /// </summary>
    public string Url { get; private set; } = null!;

    public string? Title { get; private set; }

    /// <summary>
    /// Public facing ID for the Job on Microsoft Job boards.
    /// </summary>
    public int JobNumber { get; private set; }

    public string? Location { get; private set; }

    public string? WorkSite { get; private set; }

    public string? Travel { get; private set; }

    public string? RoleType { get; private set; }

    /// <summary>
    /// Category of Job. E.g., Software Engineering
    /// </summary>
    public string? Profession { get; private set; }

    public string? Discipline { get; private set; }

    /// <summary>
    /// Full-Time, Part-Time, etc.
    /// </summary>
    public string? EmploymentType { get; private set; }

    public string? Overview { get; private set; }

    public string? Qualifications { get; private set; }

    public string? Responsibilities { get; private set; }

    /// <summary>
    /// Comma Seperated List of Benefits
    /// </summary>
    public string? Benefits { get; private set; }

    public DateTime? DatePosted { get; private set; }

    /// <summary>
    /// The Id of the <see cref="BackgroundTask"/> that created this entity.
    /// </summary>
    public BackgroundTaskId BackgroundTaskId { get; private set; } = null!;

    public static MicrosoftJobListing CreateNew(
        BackgroundTaskId backgroundTaskId,
        string title,
        int jobNumber,
        string url,
        string? location,
        DateTime? datePosted,
        string? workSite,
        string? travel,
        string? roleType,
        string? profession,
        string? discipline,
        string? employmentType,
        string? overview,
        string? qualifications,
        string? responsibilities,
        string? benefits)
    {
        return new MicrosoftJobListing
        {
            Id = new MicrosoftJobListingId(Guid.NewGuid()),
            BackgroundTaskId = backgroundTaskId,
            Title = title,
            JobNumber = jobNumber,
            DatePosted = datePosted,
            Url = url,
            Location = location,
            WorkSite = workSite,
            Travel = travel,
            RoleType = roleType,
            Profession = profession,
            Discipline = discipline,
            EmploymentType = employmentType,
            Overview = overview,
            Qualifications = qualifications,
            Responsibilities = responsibilities,
            Benefits = benefits
        };
    }

    public override bool IsValid()
    {
        throw new NotImplementedException();
    }
}
