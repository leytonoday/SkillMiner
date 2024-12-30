using SkillMiner.Application.Abstractions.CommandQueue;
using SkillMiner.Application.Services;
using SkillMiner.Domain.Entities.MicrosoftJobListingEntity;
using SkillMiner.Domain.Entities.ProfessionEntity;
using SkillMiner.Domain.Shared.Persistence;

namespace SkillMiner.Application.CQRS.Queue;

public record GenerateSkillsByProfessionQueuedCommand : QueuedCommand;

public class GenerateSkillsByProfessionQueuedCommandHandler(
    IMicrosoftJobListingRepository microsoftJobListingRepository,
    ILargeLanguageModelService largeLanguageModelService,
    IProfessionRepository professionRepository,
    IUnitOfWork unitOfWork
    ) : IQueuedCommandHandler<GenerateSkillsByProfessionQueuedCommand>
{
    public async Task Handle(GenerateSkillsByProfessionQueuedCommand request, CancellationToken cancellationToken)
    {
        // Retrieve the requirements for each profession
        IDictionary<string, List<string>> requirementsByProfession = await GetRequirementsByProfessionAsync(cancellationToken);

        // Generate the prompt used for extracting keywords
        string prompt = CreatePrompt();

        // Process each profession and its associated requirements
        foreach (var (professionName, requirements) in requirementsByProfession)
        {
            await ProcessProfessionAsync(professionName, requirements, prompt, cancellationToken);
        }

        // Commit changes to the database
        await unitOfWork.CommitAsync(cancellationToken);
    }

    private static string CreatePrompt()
    {
        // Prompt template for extracting 15 keywords from job requirements
        return @"""Extract 15 keywords from these job requirements. They should be the skills required for the job. Return a JSON array of strings.
        For example:
        [
          ""C"",
          ""C++"",
          ""C#"",
          ""Java"",
          ""JavaScript"",
          ""Python"",
          ""Distributed Systems Architecture"",
          ""System Deployment"",
          ""System Monitoring"",
          ""CS Fundamentals"",
          ""Bachelors Degree"",
          ""Masters Degree"",
          ""Technical Design"",
          ""Problem Solving"",
          ""Debugging Skills"",
          ""Software Quality Assurance"",
          ""Microsoft Azure"",
          ""AWS"",
          ""Cloud Security"",
          ""Communication Skills""
        ]
        """;
    }

    private async Task ProcessProfessionAsync(string professionName, IEnumerable<string> requirements, string prompt, CancellationToken cancellationToken)
    {
        // Retrieve the profession or create a new one if it doesn't exist
        var profession = await GetOrCreateProfessionAsync(professionName, cancellationToken);

        const int batchSize = 10; // Maximum number of requirements processed in one batch
        var requirementsList = requirements.ToList();
        int batchCount = (int)Math.Ceiling(requirementsList.Count / (double)batchSize);

        // Process requirements in batches
        for (int i = 0; i < batchCount; i++)
        {
            var batch = requirementsList.Skip(i * batchSize).Take(batchSize).ToList();
            await ProcessBatchAsync(profession, batch, prompt, cancellationToken);
        }

        // Save changes to the profession
        await SaveProfessionAsync(profession, cancellationToken);
    }

    private async Task<Profession> GetOrCreateProfessionAsync(string professionName, CancellationToken cancellationToken)
    {
        // Try to fetch an existing profession, or create a new one if it doesn't exist
        var profession = await professionRepository.GetByNameAsync(professionName, cancellationToken);
        return profession ?? Profession.Create(professionName);
    }

    private async Task ProcessBatchAsync(Profession profession, List<string> batch, string prompt, CancellationToken cancellationToken)
    {
        // Convert each requirement in the batch to a list of keywords using the LLM service
        var tasks = batch.Select(requirement =>
            largeLanguageModelService.ConvertToKeywordsAsync(requirement, prompt, 15, cancellationToken)).ToList();

        // Wait for all keyword extraction tasks to complete
        var keywordsForBatch = await Task.WhenAll(tasks);

        // Add extracted keywords to the profession
        foreach (var keywords in keywordsForBatch)
        {
            profession.AddKeywords(keywords.Select(keyword =>
                ProfessionKeyword.Create(keyword, profession.Id)).ToList());
        }
    }

    private async Task SaveProfessionAsync(Profession profession, CancellationToken cancellationToken)
    {
        // Save the profession: add if it's new, or update if it already exists
        if (profession.CreatedOnUtc == default)
        {
            await professionRepository.AddAsync(profession, cancellationToken);
        }
        else
        {
            await professionRepository.UpdateAsync(profession, cancellationToken);
        }
    }


    public async Task<IDictionary<string, List<string>>> GetRequirementsByProfessionAsync(CancellationToken cancellationToken)
    {
        var requirementsByProfession = new Dictionary<string, List<string>>();

        // Get the 50 most recent microsoft job listings
        var recentMicrosoftJobListings = (await microsoftJobListingRepository.GetPageAsync(0, 50, cancellationToken)).Item1;

        return recentMicrosoftJobListings
            .Where(x => x.Profession is not null && x.Qualifications is not null)
            .GroupBy(x => x.Profession)
            .ToDictionary(
                group => group.Key!,
                group => group.Select(listing => listing.Qualifications!).ToList()
            );
    }
}