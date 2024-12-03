using SkillMiner.Domain.Shared.Entities;

namespace SkillMiner.Domain.Entities.JobListingEntity;

public class JobListing: Entity<JobListingId>
{
    public string Title { get; private set; } 
    public string Company { get; private set; } 
    public string Location { get; private set; } 
    public string Description { get; private set; } 
    public string Url { get; private set; } 
    public DateTime PostedOnUtc { get; private set; }
    public DateTime? ClosingOnUtc { get; private set; } 
    public EmploymentType EmploymentType { get; private set; } 
    public string? Industry { get; private set; } 
    public decimal? SalaryMin { get; private set; }
    public decimal? SalaryMax { get; private set; }
    public string Currency { get; private set; }
    public string? Tags { get; private set; }

    public JobListing(
        string title,
        string company,
        string location,
        string description,
        string url,
        DateTime postedOnUtc,
        string currency,
        EmploymentType employmentType,
        decimal? salaryMin = null,
        decimal? salaryMax = null,
        DateTime? closingOnUtc = null,
        string? industry = null,
        string? tags = null
        )
    {
        Title = title;
        Company = company;
        Location = location;
        Description = description;
        Url = url;
        PostedOnUtc = postedOnUtc;
        ClosingOnUtc = closingOnUtc;
        EmploymentType = employmentType;
        Industry = industry;
        SalaryMin = salaryMin;
        SalaryMax = salaryMax;
        Currency = currency;
        CreatedOnUtc = DateTime.UtcNow;
        Tags = tags;
    }
}
