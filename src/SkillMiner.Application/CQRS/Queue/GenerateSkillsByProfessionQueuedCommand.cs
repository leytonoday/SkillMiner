using SkillMiner.Application.Abstractions.CommandQueue;
using SkillMiner.Application.Services;
using SkillMiner.Domain.Entities.MicrosoftJobListingEntity;

namespace SkillMiner.Application.CQRS.Queue;

public record GenerateSkillsByProfessionQueuedCommand : QueuedCommand;

public class GenerateSkillsByProfessionQueuedCommandHandler(
    IMicrosoftJobListingRepository microsoftJobListingRepository,
    ILargeLanguageModelService largeLanguageModelService
    ) : IQueuedCommandHandler<GenerateSkillsByProfessionQueuedCommand>
{
    public async Task Handle(GenerateSkillsByProfessionQueuedCommand request, CancellationToken cancellationToken)
    {
        var requirementsByProfession = await GetRequirementsByProfessionAsync(cancellationToken);

        string prompt = @"""Extract 15 keywords from these job requirements. They should be the skills required for the job. Return a JSON array of strings.
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

        var keywordsByProfession = new Dictionary<string, List<List<string>>>();

        foreach ((string profession, IEnumerable<string> requirements) in requirementsByProfession)
        {
            var batchSize = 10; // Adjust batch size based on your system capacity.
            var requirementsList = requirements.ToList();
            var batchCount = (int)Math.Ceiling(requirementsList.Count / (double)batchSize);

            if (!keywordsByProfession.ContainsKey(profession))
            {
                keywordsByProfession[profession] = [];
            }

            for (int i = 0; i < batchCount; i++)
            {
                // Get the current batch of requirements
                var batch = requirementsList.Skip(i * batchSize).Take(batchSize).ToList();

                // Create a list of tasks for this batch
                var tasks = batch.Select(requirement => largeLanguageModelService.ConvertToKeywordsAsync(requirement, prompt, cancellationToken)).ToList();

                // Wait for all tasks to complete in parallel
                var keywordsForBatch = await Task.WhenAll(tasks);

                // Process the results
                foreach (IEnumerable<string> keywords in keywordsForBatch)
                {
                    keywordsByProfession[profession].Add(keywords.ToList());
                }
            }
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