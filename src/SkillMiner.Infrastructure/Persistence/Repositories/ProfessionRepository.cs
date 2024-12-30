using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SkillMiner.Domain.Entities.ProfessionEntity;

namespace SkillMiner.Infrastructure.Persistence.Repositories;

public record ProfessionKeywordFrequencyDto (string ProfessionName, string Keyword, int Frequency);


public sealed class ProfessionRepository(DatabaseContext context)
    : Repository<Profession, DatabaseContext, ProfessionId>(context), IProfessionRepository
{
    public async Task<Profession?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await DbSet
            .Include(x => x.Keywords)
            .FirstOrDefaultAsync(p => p.Name == name, cancellationToken);
    }

    public async Task<Dictionary<string, List<string>>> GetTopProfessionKeywordsByFrequencyAsync(
        int limit,
        TimeSpan withinDate,
        CancellationToken cancellationToken)
    {
        var dateThreshold = DateTime.UtcNow.Subtract(withinDate);

        // Raw SQL query with ROW_NUMBER() to limit keywords per profession
        var query = @"
        WITH RankedKeywords AS (
            SELECT 
                [P].[Name] AS ProfessionName, 
                [PK].[Keyword], 
                COUNT([PK].[Keyword]) AS Frequency,
                ROW_NUMBER() OVER (PARTITION BY [P].[Name] ORDER BY COUNT([PK].[Keyword]) DESC) AS RowNum
            FROM 
                [Profession] AS [P]
            JOIN 
                [ProfessionKeyword] AS [PK]
            ON 
                [P].[Id] = [PK].[ProfessionId]
            WHERE 
                [PK].[CreatedOnUtc] >= @DateThreshold
            GROUP BY 
                [P].[Name], [PK].[Keyword]
        )
        SELECT 
            ProfessionName,
            Keyword,
            Frequency
        FROM RankedKeywords
        WHERE RowNum <= @Limit
        ORDER BY ProfessionName, Frequency DESC;";

        // Execute the query and map to DTOs
        var result = await Context.Database
            .SqlQueryRaw<ProfessionKeywordFrequencyDto>(query,
                new SqlParameter("@DateThreshold", dateThreshold),
                new SqlParameter("@Limit", limit))
            .ToListAsync(cancellationToken);

        // Transform the result into the desired structure
        var dictionary = result
            .GroupBy(x => x.ProfessionName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(x => x.Keyword).ToList());

        return dictionary;
    }

}