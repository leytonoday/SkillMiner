using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SkillMiner.Infrastructure.Persistence;

public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        // Build configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.Development.json")
            .Build();

        // Create DbContextOptionsBuilder
        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();

        // Configure DbContext 
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("SkillMiner"), options =>
        {
            options.MigrationsAssembly(typeof(DatabaseContext).Assembly.GetName().Name);
            options.MigrationsHistoryTable(HistoryRepository.DefaultTableName);
        });

        // Create and return the DbContext
        return new DatabaseContext(optionsBuilder.Options);
    }
}
